using Events.Application.Common.Auth;
using Events.Application.DTOs;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces;
using Events.Domain.Models.Users;
using Events.Domain.Models;
using MediatR;
using Events.Domain.Enums;
using Events.Application.Exceptions;
using Mapster;

namespace Events.Application.Handlers.Users;

public class UserRegistrationCommand<T>(string email,
										string password,
										Role role,
										string? firstName = null,
										string? lastName = null,
										DateTime? dateOfBirth = null) : IRequest<AuthDto> where T : class, IUser
{
	public string Email { get; private set; } = email;
	public string Password { get; private set; } = password;
	public Role Role { get; private set; } = role;
	public string? FirstName { get; private set; } = firstName;
	public string? LastName { get; private set; } = lastName;
	public DateTime? DateOfBirth { get; private set; } = dateOfBirth;
}


public class UserRegistrationCommandHandler<T>(IUsersRepository usersRepository,
													  IPasswordHash passwordHash,
													  IJwt jwt)
	: IRequestHandler<UserRegistrationCommand<T>, AuthDto> where T : class, IUser
{
	private readonly IUsersRepository _usersRepository = usersRepository;
	private readonly IPasswordHash _passwordHash = passwordHash;
	private readonly IJwt _jwt = jwt;

	public async Task<AuthDto> Handle(UserRegistrationCommand<T> request, CancellationToken cancellationToken)
	{
		var existParticipant = await _usersRepository.Get<ParticipantModel>(request.Email, cancellationToken);
		var existAdmin = await _usersRepository.Get<AdminModel>(request.Email, cancellationToken);

		if (existParticipant != null || existAdmin != null)
			throw new UserExistsException(request.Email);

		T? userModel = null;

		if (typeof(T) == typeof(ParticipantModel))
		{
			if (request.FirstName == null || request.LastName == null || request.DateOfBirth == null)
				throw new ArgumentException("First name, last name, and date of birth are required for participants.");

			userModel = new ParticipantModel(Guid.NewGuid(), request.Email, _passwordHash.Generate(request.Password), Role.User, request.FirstName, request.LastName, request.DateOfBirth.Value) as T;
		}
		else if (typeof(T) == typeof(AdminModel))
			userModel = new AdminModel(Guid.NewGuid(), request.Email, _passwordHash.Generate(request.Password), Role.Admin) as T;

		if (userModel == null)
			throw new NotFoundException($"{typeof(T)} not found");

		var accessToken = _jwt.GenerateAccessToken(userModel.Id, userModel.Role);
		var refreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(userModel.Id,
												   userModel.Role,
												   refreshToken,
												   _jwt.GetRefreshTokenExpirationDays());

		if (userModel is ParticipantModel participant)
			await _usersRepository.Create(participant, refreshTokenModel.Value, cancellationToken);
		else if (userModel is AdminModel admin)
			await _usersRepository.Create(admin, refreshTokenModel.Value, cancellationToken);

		return new AuthDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}
}

