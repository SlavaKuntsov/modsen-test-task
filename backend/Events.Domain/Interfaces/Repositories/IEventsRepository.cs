using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsRepository
{
    public Task<IList<EventModel>> Get();

    public Task<Guid> Create(EventModel eventModel);
}
