using Events.API.Contracts.Users;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Events.Persistence.Repositories;

using MapsterMapper;

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
		var user = await _usersServices.Get(request.Email, request.Password);

		if (user.IsFailure)
			return Unauthorized(user.Error);

		//generate token

		return Ok(user);
	}

	[HttpPost($"{nameof(Registration)}")]
	public async Task<IActionResult> Registration([FromBody] CreateUserRequest request)
	{
		var userModel = ParticipantModel.Create(Guid.NewGuid(), request.Email, request.Password);

		if (userModel.IsFailure)
			return BadRequest(userModel.Error);

		var user = await _usersServices.Create(userModel.Value);

		if (user.IsFailure)
			return BadRequest(user.Error);

		//generate token

		return Ok(user);
	}
}
