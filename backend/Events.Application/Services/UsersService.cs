using CSharpFunctionalExtensions;

using Events.Application.Auth;
using Events.Application.Interfaces.Auth;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;

using MapsterMapper;

namespace Events.Application.Services;

public class UsersService : IUsersServices
{
	private readonly IUsersRepository _usersRepository;
	private readonly IMapper _mapper;
	private readonly IPasswordHash _passwordHash;
	private readonly IJwt _jwt;

	public UsersService(
		IUsersRepository usersRepository,
		IMapper mapper,
		IPasswordHash passwordHash,
		IJwt jwt)
	{
		_usersRepository = usersRepository;
		_mapper = mapper;
		_passwordHash = passwordHash;
		_jwt = jwt;
	}

	public async Task<Result<string>> Login(string email, string password)
	{
		var existParticipant = await _usersRepository.Get(email);

		if (existParticipant == null)
			return Result.Failure<string>("User with this email dont exists");

		var isCorrectPassword = _passwordHash.Verify(password, existParticipant.Password);

		if (!isCorrectPassword)
			return Result.Failure<string>("Failed to login");

		var participant = _mapper.Map<ParticipantModel>(existParticipant);

		string token = _jwt.Generate(participant);

		return token;
	}

	public async Task<Result<Guid>> Register(string email, string password)
	{
		var existUser = await _usersRepository.Get(email);

		if (existUser != null)
			return Result.Failure<Guid>("User with this email already exists");

		var user = ParticipantModel.Create(Guid.NewGuid(), email, _passwordHash.Generate(password));

		if (user.IsFailure)
			return Result.Failure<Guid>(user.Error);

		return await _usersRepository.Create(user.Value);
	}
}
