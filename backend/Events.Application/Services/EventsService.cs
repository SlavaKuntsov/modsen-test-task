using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;

namespace Events.Application.Services;

public class EventsService : IEventsServices
{
	private readonly IEventsRepository _eventsRepository;

    public EventsService(IEventsRepository eventsRepository)
    {
		_eventsRepository = eventsRepository;
    }

	public async Task<IList<EventModel>> Get()
	{
		return await _eventsRepository.Get();
	}

	public async Task<Guid> Create(EventModel eventModel)
	{
		return await _eventsRepository.Create(eventModel);
	}
}
