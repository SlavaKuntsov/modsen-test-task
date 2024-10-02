using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsParticipantsRepository
{
	public Task<IList<ParticipantModel>> GetParticipantsByEvent(Guid eventId, CancellationToken cancellationToken);
	public Task<IList<EventModel>> GetEventsByParticipant(Guid participantId, CancellationToken cancellationToken);

	public Task AddEventParticipant(Guid eventId, Guid participantId, DateTime date, CancellationToken cancellationToken);

	public Task RemoveEventParticipant(Guid eventId, Guid participantId, CancellationToken cancellationToken);
	public Task RemoveParticipantFromEvents(Guid participantId, IList<EventModel> events, CancellationToken cancellationToken);

	public Task<bool> IsExists(Guid eventId, Guid participantId, CancellationToken cancellationToken);
}
