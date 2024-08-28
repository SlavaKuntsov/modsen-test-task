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

		// return GetAuthResponse
		return Ok(new GetAuthResultResponse
		{
			AccessToken = authResult.Value.AccessToken,
			RefreshToken = authResult.Value.RefreshToken
		});
	}

	[HttpPost($"{nameof(Registration)}")]
	public async Task<IActionResult> Registration([FromBody] CreateUserRequest request)
	{
		var user = await _usersServices.Register(request.Email, request.Password);

		if (user.IsFailure)
			return BadRequest(user.Error);

		return Ok(user.Value);
	}
}
