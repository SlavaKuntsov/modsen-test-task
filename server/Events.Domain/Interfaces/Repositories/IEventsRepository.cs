using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsRepository
{
	public Task<IList<EventModel>> Get(CancellationToken cancellationToken);
	public Task<IList<EventModel>> GetWithoutImage(CancellationToken cancellationToken);
	public Task<IList<EventModel>> GetEventsWithPagination(int pageNumber, int pageSize, CancellationToken cancellationToken);

	public Task<IList<Guid>> GetIds(CancellationToken cancellationToken);
	public Task<IList<Guid>> GetIdsByParticipantId(Guid id, CancellationToken cancellationToken);
	public Task<IList<Guid>> GetIdsByTitle(string title, CancellationToken cancellationToken);
	public Task<IList<Guid>> GetIdsByLocation(string location, CancellationToken cancellationToken);
	public Task<IList<Guid>> GetIdsByCategory(string category, CancellationToken cancellationToken);

	public Task<EventModel?> GetById(Guid id, CancellationToken cancellationToken);
	public Task<EventModel?> GetByIdWithoutImage(Guid id, CancellationToken cancellationToken);
	public Task<IList<EventModel>?> GetByParticipantId(Guid id, CancellationToken cancellationToken);
	public Task<IList<EventModel>?> GetByTitle(string title, CancellationToken cancellationToken);
	public Task<IList<EventModel>?> GetByLocation(string location, CancellationToken cancellationToken);
	public Task<IList<EventModel>?> GetByCategory(string category, CancellationToken cancellationToken);

	public Task<Guid> Create(EventModel eventModel, CancellationToken cancellationToken);

	public Task<Guid> Update(EventModel eventModel, CancellationToken cancellationToken);

	public Task Delete(Guid eventId, CancellationToken cancellationToken);

	public Task<bool> IsExists(Guid eventId, CancellationToken cancellationToken);
}
