using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;

using MediatR;

namespace Events.Application.Handlers.Events;

public class AddParticipantToEventCommand(string eventId, string participantId) : IRequest
{
	public Guid EventId { get; private set; } = Guid.Parse(eventId);
	public Guid ParticipantId { get; private set; } = Guid.Parse(participantId);
}

public class AddParticipantToEventCommandHandler(IEventsParticipantsRepository eventsParticipantsRepository) : IRequestHandler<AddParticipantToEventCommand>
{
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository = eventsParticipantsRepository;

	public async Task Handle(AddParticipantToEventCommand request, CancellationToken cancellationToken)
	{
		if (await _eventsParticipantsRepository.IsExists(request.EventId, request.ParticipantId))
			throw new RegistrationExistsException("Same registration already exists");

		DateTime dateTime = DateTime.Now;

		await _eventsParticipantsRepository.AddEventParticipant(request.EventId, request.ParticipantId, dateTime);
	}
}
