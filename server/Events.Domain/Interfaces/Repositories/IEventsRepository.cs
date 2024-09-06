using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsRepository
{
	public Task<IList<EventModel>> Get();
	public Task<EventModel?> Get(Guid id);
	public Task<IList<EventModel>?> GetByParticipantId(Guid id);
	public Task<IList<EventModel>?> GetByTitle(string title);
	public Task<IList<EventModel>?> GetByLocation(string location);
	public Task<IList<EventModel>?> GetByCategory(string category);
	//public Task<EventModel?> Get<T>(T value, Func<EventEntity, bool> predicate);

	public Task<Guid> Create(EventModel eventModel);

	public Task<Guid> Update(EventModel eventModel);

	public Task Delete(Guid eventId);

	public Task<bool> IsExists(Guid eventId);
}
