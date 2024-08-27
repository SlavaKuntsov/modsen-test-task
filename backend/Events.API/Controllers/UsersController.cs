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
		var token = await _usersServices.Login(request.Email, request.Password);

		if (token.IsFailure)
			return Unauthorized(token.Error);

		HttpContext.Response.Cookies.Append(ApiExtensions.COOKIE_NAME, token.Value);

		return Ok(token.Value);
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
