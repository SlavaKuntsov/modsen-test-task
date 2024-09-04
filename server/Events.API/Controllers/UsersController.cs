﻿using Events.API.Contracts.Participants;
using Events.API.Contracts.Users;
using Events.Application.Services;
using Events.Domain.Enums;
using Events.Domain.Interfaces.Services;

using MapsterMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class UsersController : BaseController
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
			return BadRequest("Such role does not exist");

		if (role != Role.Admin)
			return (BadRequest("Role does not equal the necessary one"));

		var authResult = await _usersServices.AdminRegistration(request.Email, request.Password, role);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpGet(nameof(AdminActivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminActivation(Guid id)
	{
		var model = await _usersServices.ChangeAdminActivation(id, true);

		if (model.IsFailure)
			return BadRequest(model.Error);

		var result = _mapper.Map<GetAuthResultResponse>(model.Value);

		return Ok(result);
	}

	[HttpGet(nameof(AdminDeactivation) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> AdminDeactivation(Guid id)
	{
		var model = await _usersServices.ChangeAdminActivation(id, false);

		if (model.IsFailure)
			return BadRequest(model.Error);

		var result = _mapper.Map<GetAuthResultResponse>(model.Value);

		return Ok(result);
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
		var userIdClaim = User.FindFirst("Id");

		if (userIdClaim == null)
			return Unauthorized("User ID not found in claims.");

		Guid userId = Guid.Parse(userIdClaim.Value);

		var user = await _usersServices.GetOrAuthorize(userId);

		if (user.IsFailure)
			return Unauthorized(user.Error);

		var response = _mapper.Map<GetParticipantResponse>(user.Value);

		return Ok(response);
	}

	[HttpGet(nameof(Unauthorize))]
	//[Authorize(Policy = "UserOrAdmin")]
	public IActionResult Unauthorize()
	{
		HttpContext.Response.Cookies.Delete(ApiExtensions.COOKIE_NAME);

		return Ok();
	}

	[HttpGet(nameof(GetParticipant) + "/{id:Guid}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetParticipant(Guid id)
	{
		var user = await _usersServices.GetOrAuthorize(id);

		if (user.IsFailure)
			return Unauthorized(user.Error);

		var response = _mapper.Map<GetParticipantResponse>(user.Value);

		return Ok(response);
	}
}
