using System.Security.Claims;

using Events.API.Contracts.Participants;
using Events.API.Contracts.Users;
using Events.API.Extensions;
using Events.Domain.Enums;
using Events.Domain.Interfaces.Services;

using MapsterMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly IUsersServices _usersServices;
	private readonly IMapper _mapper;

	public UsersController(IUsersServices usersServices, IMapper mapper)
	{
		_usersServices = usersServices;
		_mapper = mapper;
	}

	[HttpPost(nameof(Login))]
	public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
	{
		var authResult  = await _usersServices.Login(request.Email, request.Password);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpPost(nameof(ParticipantRegistration))]
	public async Task<IActionResult> ParticipantRegistration([FromBody] CreateParticipantRequest request)
	{
		if (!Enum.TryParse<Role>(request.Role, out var role))
			return BadRequest("Such role does not exist");

		if (role != Role.User)
			return (BadRequest("Role does not equal the necessary one"));

		var authResult = await _usersServices.ParticipantRegistration(request.Email, request.Password, role, request.FirstName, request.LastName, request.DateOfBirth);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpPost(nameof(AdminRegistration))]
	public async Task<IActionResult> AdminRegistration([FromBody] CreateAdminRequest request)
	{
		if (!Enum.TryParse<Role>(request.Role, out var role))
			//throw new BadHttpRequestException("Such role does not exist");
			return BadRequest("Such role does not exist");

		if (role != Role.Admin)
			return BadRequest("Role does not equal the necessary one");

		var authResult = await _usersServices.AdminRegistration(request.Email, request.Password, role);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpPut(nameof(Update))]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Update([FromBody] UpdateParticipantRequest request)
	{
		var particantModel =  await _usersServices.Update(request.Id, request.FirstName, request.LastName, request.DateOfBirth);

		if (particantModel.IsFailure)
			return BadRequest(particantModel.Error);

		var result = _mapper.Map<GetParticipantResponse>(particantModel.Value);

		return Ok(result);
	}

	[HttpDelete(nameof(Delete) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var user =  await _usersServices.Delete(id);

		if (user.IsFailure)
			return BadRequest(user.Error);

		return Ok();
	}

	[HttpGet(nameof(AdminActivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminActivation(Guid id)
	{
		var model = await _usersServices.ChangeAdminActivation(id, true);

		if (model.IsFailure)
			return BadRequest(model.Error);

		//var result = _mapper.Map<GetAuthResultResponse>(model.Value);

		return Ok();
	}

	[HttpGet(nameof(AdminDeactivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminDeactivation(Guid id)
	{
		var model = await _usersServices.ChangeAdminActivation(id, false);

		if (model.IsFailure)
			return BadRequest(model.Error);

		//var result = _mapper.Map<GetAuthResultResponse>(model.Value);

		return Ok();
	}

	[HttpPost(nameof(RefreshToken))]
	public async Task<IActionResult> RefreshToken()
	{
		string? refreshToken = HttpContext.Request.Cookies[ApiExtensions.COOKIE_NAME];

		if (string.IsNullOrEmpty(refreshToken))
			return Unauthorized("Refresh token is missing.");

		var authResult = await _usersServices.RefreshToken(refreshToken);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpGet(nameof(Authorize))]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> Authorize()
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
		//var userRoleClaim = User.FindFirst(ClaimTypes.Role);

		if (userIdClaim == null)
			return Unauthorized("User ID not found in claims.");

		Guid userId = Guid.Parse(userIdClaim.Value);

		var participant = await _usersServices.GetOrAuthorize(userId);
		var admin = await _usersServices.GetOrAuthorizeAdmin(userId);

		if (participant.IsSuccess)
			return Ok(_mapper.Map<GetParticipantResponse>(participant.Value));
		if (admin.IsSuccess)
			return Ok(_mapper.Map<GetParticipantResponse>(admin.Value));
		else
			return BadRequest(participant.Error);
	}

	[HttpGet(nameof(Unauthorize))]
	//[Authorize(Policy = "UserOrAdmin")]
	public IActionResult Unauthorize()
	{
		HttpContext.Response.Cookies.Delete(ApiExtensions.COOKIE_NAME);

		return Ok();
	}

	[HttpGet(nameof(GetParticipant) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetParticipant(Guid id)
	{
		var user = await _usersServices.GetOrAuthorize(id);

		if (user.IsFailure)
			return Unauthorized(user.Error);

		var response = _mapper.Map<GetParticipantResponse>(user.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetAdmins))]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetAdmins()
	{
		var adminModels = await _usersServices.GetAdmins();
		var responses = _mapper.Map<IList<GetAdminResponse>>(adminModels);

		return Ok(responses);
	}
}
