using Events.Application.Common.Auth;
using Events.Domain.Models.Users;
using MediatR;
using Events.Domain.Interfaces;

namespace Events.Application.Handlers.Users;

public class LoginUserQuery<T>(T user, string password) : IRequest where T : IUser
{
	public T User { get; private set; } = user;
	public string Password { get; private set; } = password;
}

public class LoginUserCommandHandler<T>(IPasswordHash passwordHash) : IRequestHandler<LoginUserQuery<T>> where T : IUser
{
	private readonly IPasswordHash _passwordHash = passwordHash;

	public async Task Handle(LoginUserQuery<T> request, CancellationToken cancellationToken)
	{
		if (request.User is AdminModel admin && !admin.IsActiveAdmin)
			throw new UnauthorizedAccessException("Admin doesn't have active role");

		var isCorrectPassword = _passwordHash.Verify(request.Password, request.User.Password);

		if (!isCorrectPassword)
			throw new UnauthorizedAccessException("Incorrect password");
	}
}