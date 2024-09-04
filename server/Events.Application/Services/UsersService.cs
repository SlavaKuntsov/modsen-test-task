using CSharpFunctionalExtensions;

using Events.Application.Auth;
using Events.Application.Interfaces.Auth;
using Events.Domain.Enums;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Events.Domain.Models.Users;

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

	public async Task<Result<AuthResultModel>> Login(string email, string password)
	{
		var existUser = await _usersRepository.Get(email);

		if (existUser == null)
			return Result.Failure<AuthResultModel>("User with this email doesn't exists");

		var isCorrectPassword = _passwordHash.Verify(password, existUser.Password);

		if (!isCorrectPassword)
			return Result.Failure<AuthResultModel>("Incorrect password");

		var user = _mapper.Map<ParticipantModel>(existUser);

		string accessToken = _jwt.GenerateAccessToken(user.Id, user.Role);
		string refreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(user.Id, user.Role, refreshToken, _jwt.GetRefreshTokenExpirationDays());

		if (refreshTokenModel.IsFailure)
			return Result.Failure<AuthResultModel>(refreshTokenModel.Error);

		await _usersRepository.UpdateRefreshToken(user.Id, user.Role, refreshTokenModel.Value);

		return new AuthResultModel
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
		};
	}

	public async Task<Result<AuthResultModel>> ParticipantRegistration(string email, string password, Role role, string firstName, string lastName, string dateOfBirth)
	{
		var existUser = await _usersRepository.Get(email);

		if (existUser != null)
			return Result.Failure<AuthResultModel>("User with this email already exists");

		var user = ParticipantModel.Create(Guid.NewGuid(), email, _passwordHash.Generate(password), role, firstName, lastName, dateOfBirth);
		if (user.IsFailure)
			return Result.Failure<AuthResultModel>(user.Error);

		var accessToken = _jwt.GenerateAccessToken(user.Value.Id, user.Value.Role);
		var refreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(user.Value.Id, Role.User, refreshToken, _jwt.GetRefreshTokenExpirationDays());

		if (refreshTokenModel.IsFailure)
			return Result.Failure<AuthResultModel>(refreshTokenModel.Error);

		var createdUserId = await _usersRepository.Create(user.Value, refreshTokenModel.Value);

		if (createdUserId.IsFailure)
			return Result.Failure<AuthResultModel>(createdUserId.Error);

		return new AuthResultModel
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}

	public async Task<Result<AuthResultModel>> AdminRegistration(string email, string password, Role role)
	{
		var existUser = await _usersRepository.Get(email);

		if (existUser != null)
			return Result.Failure<AuthResultModel>("User with this email already exists");

		var user = AdminModel.Create(Guid.NewGuid(), email, _passwordHash.Generate(password), role);

		if (user.IsFailure)
			return Result.Failure<AuthResultModel>(user.Error);

		var accessToken = _jwt.GenerateAccessToken(user.Value.Id, user.Value.Role);
		var refreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(user.Value.Id, Role.Admin, refreshToken, _jwt.GetRefreshTokenExpirationDays());

		if (refreshTokenModel.IsFailure)
			return Result.Failure<AuthResultModel>(refreshTokenModel.Error);

		var createdUserId = await _usersRepository.Create(user.Value, refreshTokenModel.Value);

		return new AuthResultModel
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}

	public async Task<Result<AdminModel>> ChangeAdminActivation(Guid id, bool isActive)
	{
		var existUser = await _usersRepository.Get(id);

		if (existUser == null)
			return Result.Failure<AdminModel>("User with this id doesn't exists");

		return await _usersRepository.ChangeAdminActivation(id, isActive);
	}

	public async Task<Result<AuthResultModel>> RefreshToken(string refreshToken)
	{
		// TODO - как варинат достать из токена, из Claims, айди что там находит
		var userId = await _jwt.ValidateRefreshToken(refreshToken);

		if (userId == Guid.Empty)
			return Result.Failure<AuthResultModel>("Invalid refresh token");

		var user = await _usersRepository.Get(userId);

		if (user == null)
			return Result.Failure<AuthResultModel>("User not found");

		var accessToken = _jwt.GenerateAccessToken(user.Id, user.Role);
		var newRefreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(user.Id, user.Role, refreshToken, _jwt.GetRefreshTokenExpirationDays());

		if (refreshTokenModel.IsFailure)
			return Result.Failure<AuthResultModel>(refreshTokenModel.Error);

		await _usersRepository.UpdateRefreshToken(user.Id, user.Role, refreshTokenModel.Value);

		return new AuthResultModel
		{
			AccessToken = accessToken,
			RefreshToken = newRefreshToken
		};
	}

	public async Task<Result<ParticipantModel>> GetOrAuthorize(Guid id)
	{
		var user = await _usersRepository.Get(id);

		if (user == null)
			return Result.Failure<ParticipantModel>("User not found");

		return user;
	}
}
