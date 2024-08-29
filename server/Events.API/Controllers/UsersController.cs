using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Events.API.Contracts.Users;
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

	[HttpPost($"{nameof(Login)}")]
	public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
	{
		var authResult  = await _usersServices.Login(request.Email, request.Password);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpPost($"{nameof(Registration)}")]
	public async Task<IActionResult> Registration([FromBody] CreateUserRequest request)
	{
		var authResult = await _usersServices.Registration(request.Email, request.Password);

		if (authResult.IsFailure)
			return Unauthorized(authResult.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, authResult.Value.RefreshToken);

		var result = _mapper.Map<GetAuthResultResponse>(authResult.Value);

		return Ok(result);
	}

	[HttpPost($"{nameof(RefreshToken)}")]
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

	[HttpGet($"{nameof(Authorize)}")]
	[Authorize]
	public async Task<IActionResult> Authorize()
	{
		var userIdClaim = User.FindFirst("Id");

		if (userIdClaim == null)
			return Unauthorized("User ID not found in claims.");

		Guid userId = Guid.Parse(userIdClaim.Value);

		var user = await _usersServices.Authorize(userId);

		if (user.IsFailure)
			return Unauthorized(user.Error);

		return Ok(user.Value);
	}

	[HttpGet($"{nameof(Unauthorize)}")]
	//[Authorize]
	public IActionResult Unauthorize()
	{
		HttpContext.Response.Cookies.Delete(ApiExtensions.COOKIE_NAME);

		return Ok();
	}
}
