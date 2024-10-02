using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;

using MediatR;

namespace Events.Application.Handlers.Events;

public class RemoveParticipantFromEventCommand(string eventId, string participantId) : IRequest
{
	public Guid EventId { get; private set; } = Guid.Parse(eventId);
	public Guid ParticipantId { get; private set; } = Guid.Parse(participantId);
}

public class RemoveParticipantFromEventCommandHandler(IEventsParticipantsRepository eventsParticipantsRepository) : IRequestHandler<RemoveParticipantFromEventCommand>
{
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository = eventsParticipantsRepository;

	public async Task Handle(RemoveParticipantFromEventCommand request, CancellationToken cancellationToken)
	{
		if (!await _eventsParticipantsRepository.IsExists(request.EventId, request.ParticipantId, cancellationToken))
			throw new RegistrationExistsException("Same registration doesn't exists");

		await _eventsParticipantsRepository.RemoveEventParticipant(request.EventId, request.ParticipantId, cancellationToken);
	}
}
