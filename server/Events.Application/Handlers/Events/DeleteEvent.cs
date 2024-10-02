using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;

using MediatR;

namespace Events.Application.Handlers.Events;

public class DeleteEventCommand(Guid id) : IRequest
{
	public Guid Id { get; private set; } = id;
}

public class DeleteEventCommandHandler(IEventsRepository eventsRepository) : IRequestHandler<DeleteEventCommand>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;

	public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
	{
		if (!await _eventsRepository.IsExists(request.Id))
			throw new RegistrationExistsException("Event with this id doesn't exists");

		await _eventsRepository.Delete(request.Id);
	}
}
