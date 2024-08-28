using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;

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

	public async Task<Result<AuthResult>> Login(string email, string password)
	{
		var existParticipant = await _usersRepository.Get(email);

		if (existParticipant == null)
			return Result.Failure<AuthResult>("User with this email doesn't exists");

		var isCorrectPassword = _passwordHash.Verify(password, existParticipant.Password);

		if (!isCorrectPassword)
			return Result.Failure<AuthResult>("Failed to login");

		var participant = _mapper.Map<ParticipantModel>(existParticipant);

		var accessToken = _jwt.GenerateAccessToken(participant);
		var refreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(Guid.NewGuid(), refreshToken, _jwt.GetRefreshTokenExpirationDays());

		if (refreshTokenModel.IsFailure)
			return Result.Failure<AuthResult>(refreshTokenModel.Error);

		await _usersRepository.SaveRefreshToken(refreshTokenModel.Value);

		//string token = _jwt.Generate(participant);

		return new AuthResult
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
		};
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

	public async Task<Result<AuthResult>> RefreshToken(string refreshToken)
	{
		// TODO - как варинат достать из токена, из Claims, айди что там находит
		var userId = await _jwt.ValidateRefreshToken(refreshToken);

		if (userId == Guid.Empty)
			return Result.Failure<AuthResult>("Invalid refresh token");

		var user = await _usersRepository.Get(userId);

		if (user == null)
			return Result.Failure<AuthResult>("User not found");

		var accessToken = _jwt.GenerateAccessToken(user);
		var newRefreshToken = _jwt.GenerateRefreshToken();

		// Обновление refresh-токена в хранилище
		await _usersRepository.UpdateRefreshToken(userId, newRefreshToken);

		return Result.Success(new AuthResult
		{
			AccessToken = accessToken,
			RefreshToken = newRefreshToken
		});
	}
}
