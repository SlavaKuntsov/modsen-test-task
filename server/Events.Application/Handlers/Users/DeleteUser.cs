using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces;
using Events.Domain.Models.Users;
using MediatR;

namespace Events.Application.Handlers.Users;

public class DeleteUserCommand<T>(Guid id) : IRequest where T : class, IUser
{
	public Guid Id { get; private set; } = id;
}

public class DeleteUserCommandHandler<T>(IUsersRepository usersRepository,
										 IEventsParticipantsRepository eventsParticipantsRepository) : IRequestHandler<DeleteUserCommand<T>> where T : class, IUser
{
	private readonly IUsersRepository _usersRepository = usersRepository;
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository = eventsParticipantsRepository;

	public async Task Handle(DeleteUserCommand<T> request, CancellationToken cancellationToken)
	{
		if (typeof(T) == typeof(ParticipantModel))
		{
			var events = _eventsParticipantsRepository.GetEventsByParticipant(request.Id);

			if (events.Result.Count == 0)
				return;

			await _eventsParticipantsRepository.RemoveParticipantFromEvents(request.Id, events.Result);
			await _usersRepository.Delete<ParticipantModel>(request.Id);
			return;
		}
		else if (typeof(T) == typeof(AdminModel))
		{
			await _usersRepository.Delete<AdminModel>(request.Id);
			return;
		}
	}
}

