using Events.Domain.Interfaces;
using Events.Domain.Interfaces.Repositories;

using MediatR;

namespace Events.Application.Handlers.Users
{
	public class GetUserByFilterQuery<T>(Guid? userId = null, string? email = null) : IRequest<T?> where T : class, IUser
	{
		public Guid? UserId { get; private set; } = userId;
		public string? Email { get; private set; } = email;
	}

	public class GetUserByFilterQueryHandler<T>(IUsersRepository usersRepository) : IRequestHandler<GetUserByFilterQuery<T>, T?> where T : class, IUser
	{
		private readonly IUsersRepository _usersRepository = usersRepository;

		public async Task<T?> Handle(GetUserByFilterQuery<T> request, CancellationToken cancellationToken)
		{
			T? userModel = null;

			if (request.UserId.HasValue)
				userModel = await _usersRepository.Get<T>(request.UserId.Value);
			else if (!string.IsNullOrEmpty(request.Email))
				userModel = await _usersRepository.Get<T>(request.Email);

			return userModel;
		}
	}
}
