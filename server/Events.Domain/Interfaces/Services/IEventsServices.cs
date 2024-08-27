using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IEventsServices
{
	public Task<IList<EventModel>> Get();

	public Task<Guid> Create(EventModel eventModel);
}
