using Events.API.Contracts.Users;
using Events.Domain.Interfaces;
using Events.Domain.Models;
using Events.Persistence.Repositories;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class UsersController : BaseController
{
	private readonly IUsersRepository _usersRepository;
	private readonly IMapper _mapper;

	public UsersController(IUsersRepository usersRepository, IMapper mapper)
	{
		_usersRepository = usersRepository;
		_mapper = mapper;
	}

	[HttpPost($"{nameof(Login)}")]
	public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
	{
		var user = await _usersRepository.Get(request.Email, request.Password);

		//if (user.isFailure)
		if (user == null)
			return Unauthorized();

	   //generate token

		return Ok(user);
	}

	[HttpPost($"{nameof(Registration)}")]
	public async Task<IActionResult> Registration([FromBody] CreateUserRequest request)
	{
		var user = await _usersRepository.Get(request.Email);

		//if (user.isFailure)
		if (user != null)
			return BadRequest("User with this email already exists");


		var userModel = ParticipantModel.Create(Guid.NewGuid(), request.Email, request.Password);

		if (userModel.IsFailure)
			return BadRequest(userModel.Error);

		var userId = await _usersRepository.Create(userModel.Value);

	   //generate token

		return Ok(user);
	}
}
