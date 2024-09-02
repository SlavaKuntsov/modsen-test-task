using CSharpFunctionalExtensions;

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

	public async Task<Result<EventModel>> Get(Guid id)
	{
		var existEvent = await _eventsRepository.Get(id);

		if (existEvent == null)
			return Result.Failure<EventModel>("Event with this id doesn't exists");

		return existEvent;
	}

	public async Task<Result<Guid>> Create(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, string imageUrl)
	{

		var user = EventModel.Create(Guid.NewGuid(),  title, description, eventDateTime, location, category, maxParticipants, imageUrl);

		if (user.IsFailure)
			return Result.Failure<Guid>(user.Error);

		var createdEventId = await _eventsRepository.Create(user.Value);

		return createdEventId;
	}
}
