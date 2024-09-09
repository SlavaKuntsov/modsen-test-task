using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsParticipantsRepository
{
	public Task<IList<ParticipantModel>> GetParticipantsByEvent(Guid eventId);
	public Task<IList<EventModel>> GetEventsByParticipant(Guid participantId);

	public Task AddEventParticipant(Guid eventId, Guid participantId, DateTime date);

	public Task RemoveEventParticipant(Guid eventId, Guid participantId);
	public Task RemoveParticipantFromEvents(Guid participantId, IList<EventModel> events);

	public Task<bool> IsExists(Guid eventId, Guid participantId);
}
