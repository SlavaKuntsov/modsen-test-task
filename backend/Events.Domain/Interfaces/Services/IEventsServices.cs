using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IEventsServices
{
	public Task<ICollection<EventModel>> Get();

	public Task<Guid> Create(EventModel eventModel);
}
