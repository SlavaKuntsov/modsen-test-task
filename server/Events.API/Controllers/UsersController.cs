using System.Globalization;
using System.Security.Claims;

using Events.API.Contracts.Participants;
using Events.API.Contracts.Users;
using Events.API.Extensions;
using Events.Application.DTOs;
using Events.Application.Exceptions;
using Events.Application.Handlers.Tokens;
using Events.Application.Handlers.Users;
using Events.Domain.Constants;
using Events.Domain.Enums;
using Events.Domain.Models.Users;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public UsersController(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	[HttpPost(nameof(Login))]
	public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
	{
		var participantModel = await _mediator.Send(new GetUserByFilterQuery<ParticipantModel>(email: request.Email));

		if (participantModel != null)
		{
			await _mediator.Send(new LoginUserQuery<ParticipantModel>(participantModel, request.Password));

			var authResultDto = await _mediator.Send(new GenerateAndUpdateTokensCommand(participantModel.Id, participantModel.Role));

			HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResultDto.RefreshToken);

			return Ok(authResultDto);
		}

		var adminModel = await _mediator.Send(new GetUserByFilterQuery<AdminModel>(email: request.Email));

		if (adminModel != null)
		{
			await _mediator.Send(new LoginUserQuery<AdminModel>(adminModel, request.Password));

			var authResultDto = await _mediator.Send(new GenerateAndUpdateTokensCommand(adminModel.Id, adminModel.Role));

			HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResultDto.RefreshToken);

			return Ok(authResultDto);
		}

		return Unauthorized("User with this email doesn't exists");
	}

	[HttpPost(nameof(ParticipantRegistration))]
	public async Task<IActionResult> ParticipantRegistration([FromBody] CreateParticipantRequest request)
	{
		if (!Enum.TryParse<Role>(request.Role, out var role))
			return BadRequest("Such role does not exist");

		if (role != Role.User)
			return (BadRequest("Role does not equal the necessary one"));

		if (!DateTime.TryParseExact(request.DateOfBirth, DateTimeConst.DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
			return BadRequest("Invalid date format.");

		var authResult = await _mediator.Send(new UserRegistrationCommand<ParticipantModel>(request.Email, request.Password, role, request.FirstName, request.LastName, parsedDateTime));

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.RefreshToken);

		return Ok(authResult);
	}

	[HttpPost(nameof(AdminRegistration))]
	public async Task<IActionResult> AdminRegistration([FromBody] CreateAdminRequest request)
	{
		if (!Enum.TryParse<Role>(request.Role, out var role))
			return BadRequest("Such role does not exist");

		if (role != Role.Admin)
			return BadRequest("Role does not equal the necessary one");

		var authResult = await _mediator.Send(new UserRegistrationCommand<AdminModel>(request.Email, request.Password, role));

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.RefreshToken);

		return Ok(authResult);
	}

	[HttpPut(nameof(Update))]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Update([FromBody] UpdateParticipantRequest request)
	{
		if (!DateTime.TryParseExact(request.DateOfBirth, DateTimeConst.DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
			return BadRequest("Invalid date format.");

		var particantModel =  await _mediator.Send(new UpdateParticipantCommand(request.Id, request.FirstName, request.LastName, parsedDateTime));

		return Ok(particantModel);
	}

	[HttpDelete(nameof(Delete) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var participantModel = await _mediator.Send(new GetUserByFilterQuery<ParticipantModel>(userId: id));

		if (participantModel != null)
		{
			await _mediator.Send(new DeleteUserCommand<ParticipantModel>(id));
			return Ok();
		}

		var adminModel = await _mediator.Send(new GetUserByFilterQuery<AdminModel>(userId: id));

		if (adminModel != null)
		{
			await _mediator.Send(new DeleteUserCommand<AdminModel>(id));
			return Ok();
		}

		return Unauthorized("User with this id doesn't exists");
	}

	[HttpGet(nameof(AdminActivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminActivation(Guid id)
	{
		await _mediator.Send(new ChangeAdminActivationCommand(id, true));

		return Ok();
	}

	[HttpGet(nameof(AdminDeactivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminDeactivation(Guid id)
	{
		await _mediator.Send(new ChangeAdminActivationCommand(id, false));

		return Ok();
	}

	[HttpPost(nameof(RefreshToken))]
	public async Task<IActionResult> RefreshToken()
	{
		string? refreshToken = HttpContext.Request.Cookies[ApiExtensions.COOKIE_NAME];

		if (string.IsNullOrEmpty(refreshToken))
			return Unauthorized("Refresh token is missing.");

		var userDto = await _mediator.Send(new RefreshTokenCommand(refreshToken));

		var authResult = await _mediator.Send(new GenerateAndUpdateTokensCommand(userDto.Id, userDto.Role));

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.RefreshToken);

		return Ok(authResult);
	}

	[HttpGet(nameof(Authorize))]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Authorize()
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

		if (userIdClaim == null)
			return Unauthorized("User ID not found in claims.");

		Guid userId = Guid.Parse(userIdClaim.Value);

		var participant = await _mediator.Send(new GetOrAuthorizeUserQuery<ParticipantModel>(userId));

		if (participant != null)
			return Ok(_mapper.Map<ParticipantDto>(participant));

		var admin = await _mediator.Send(new GetOrAuthorizeUserQuery<AdminModel>(userId));

		if (admin != null)
			return Ok(_mapper.Map<AdminDto>(admin));

		return BadRequest("User not found");
	}

	[HttpGet(nameof(Unauthorize))]
	public IActionResult Unauthorize()
	{
		HttpContext.Response.Cookies.Delete(ApiExtensions.COOKIE_NAME);

		return Ok();
	}

	[HttpGet(nameof(GetParticipant) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetParticipant(Guid id)
	{
		var user = await _mediator.Send(new GetOrAuthorizeUserQuery<ParticipantModel>(id));

		if (user == null)
			throw new NotFoundException($"Participant not found");

		return Ok(_mapper.Map<ParticipantDto>(user));
	}

	[HttpGet(nameof(GetParticipants))]
	//[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetParticipants()
	{
		var adminModels = await _mediator.Send(new GetUsersQuery<ParticipantModel>());
		var responses = _mapper.Map<IList<ParticipantDto>>(adminModels);

		return Ok(responses);
	}

	[HttpGet(nameof(GetAdmins))]
	//[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetAdmins()
	{
		var adminModels = await _mediator.Send(new GetUsersQuery<AdminModel>());
		var responses = _mapper.Map<IList<AdminDto>>(adminModels);

		return Ok(responses);
	}
}
