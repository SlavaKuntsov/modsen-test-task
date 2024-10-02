using Events.Domain.Interfaces;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models.Users;

using MediatR;

namespace Events.Application.Handlers.Users;

public class GetUsersQuery<T>() : IRequest<IList<T>> where T : class, IUser
{
}

public class GetUsersQueryHandler<T>(IUsersRepository usersRepository) : IRequestHandler<GetUsersQuery<T>, IList<T>> where T : class, IUser
{
	private readonly IUsersRepository _usersRepository = usersRepository;

	public async Task<IList<T>> Handle(GetUsersQuery<T> request, CancellationToken cancellationToken)
	{
		IList<T>? userModel = [];

		if (typeof(T) == typeof(ParticipantModel))
			userModel = await _usersRepository.Get<ParticipantModel>() as IList<T>;
		else if (typeof(T) == typeof(AdminModel))
			userModel = await _usersRepository.Get<AdminModel>() as IList<T>;

		if (userModel == null || !userModel.Any())
			return [];

		return userModel;
	}
}