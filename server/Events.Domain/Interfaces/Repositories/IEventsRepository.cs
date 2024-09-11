using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsRepository
{
	public Task<IList<EventModel>> Get();
	public Task<IList<EventModel>> GetWithoutImage();

	public Task<IList<Guid>> GetIds();
	public Task<IList<Guid>> GetIdsByParticipantId(Guid id);
	public Task<IList<Guid>> GetIdsByTitle(string title);
	public Task<IList<Guid>> GetIdsByLocation(string location);
	public Task<IList<Guid>> GetIdsByCategory(string category);

	public Task<EventModel?> GetById(Guid id);
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
