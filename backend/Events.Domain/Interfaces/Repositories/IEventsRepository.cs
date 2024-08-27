using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventsRepository
{
    public Task<ICollection<EventModel>> Get();

    public Task<Guid> Create(EventModel eventModel);
}
