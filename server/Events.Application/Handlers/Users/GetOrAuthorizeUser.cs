using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces;

using MediatR;
using Events.Domain.Models.Users;

namespace Events.Application.Handlers.Users;

public class GetOrAuthorizeUserQuery<T>(Guid id) : IRequest<T?>
{
	public Guid Id { get; set; } = id;
}

public class GetOrAuthorizeUserQueryHandler<T>(IUsersRepository usersRepository) : IRequestHandler<GetOrAuthorizeUserQuery<T>, T?> where T : class, IUser
{
	private readonly IUsersRepository _usersRepository = usersRepository;

	public async Task<T?> Handle(GetOrAuthorizeUserQuery<T> request, CancellationToken cancellationToken)
	{
		T? userModel = null;

		if (typeof(T) == typeof(ParticipantModel))
			userModel = await _usersRepository.Get<ParticipantModel>(request.Id, cancellationToken) as T;
		else if (typeof(T) == typeof(AdminModel))
			userModel = await _usersRepository.Get<AdminModel>(request.Id, cancellationToken) as T;

		// Не выкидываем исключение потому что нам надо перейти к коду ниже в родительском блоке
		//if (userModel == null)
		//	throw new NotFoundException($"{typeof(T)} not found");

		return userModel;
	}
}