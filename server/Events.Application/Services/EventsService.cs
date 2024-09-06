using CSharpFunctionalExtensions;

using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Application.Services;

public class EventsService : IEventsServices
{
	private readonly IEventsRepository _eventsRepository;
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository;

	public EventsService(IEventsRepository eventsRepository, IEventsParticipantsRepository eventsParticipantsRepository)
	{
		_eventsRepository = eventsRepository;
		_eventsParticipantsRepository = eventsParticipantsRepository;
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

	public async Task<Result<IList<EventModel>>> GetByParticipantId(Guid id)
	{
		var existEvents = await _eventsRepository.GetByParticipantId(id);

		if (existEvents == null)
			//return Result.Failure<IList<EventModel>>("Events for this participant doesn't exists");
			return Result.Success<IList<EventModel>>([]);

		return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByTitle(string title)
	{
		var existEvents = await _eventsRepository.GetByTitle(title);

		if (existEvents == null)
			return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByLocation(string location)
	{
		var existEvents = await _eventsRepository.GetByLocation(location);

		if (existEvents == null)
			return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByCategory(string category)
	{
		var existEvents = await _eventsRepository.GetByCategory(category);

		if (existEvents == null)
			return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		return Result.Success(existEvents);
	}

	public async Task<Result<IList<ParticipantModel>>> GetEventParticipants(Guid eventId)
	{
		var particants = await _eventsParticipantsRepository.GetParticipantsByEvent(eventId);

		if (particants == null)
			return Result.Failure<IList<ParticipantModel>>("Particants not found");

		return Result.Success(particants);
	}

	public async Task<Result<Guid>> Create(string title, string description, string eventDateTime, string location, string category, int maxParticipants, string imageUrl)
	{
		var eventModel = EventModel.Create(Guid.NewGuid(),  title, description, eventDateTime, location, category, maxParticipants, imageUrl);

		if (eventModel.IsFailure)
			return Result.Failure<Guid>(eventModel.Error);

		return await _eventsRepository.Create(eventModel.Value);
	}

	public async Task<Result> AddParticipantToEvent(string eventId, string participantId)
	{
		if (await _eventsParticipantsRepository.IsExists(Guid.Parse(eventId), Guid.Parse(participantId)))
			return Result.Failure("Same registration already exists");

		DateTime dateTime = DateTime.Now;

		await _eventsParticipantsRepository.AddEventParticipant(Guid.Parse(eventId), Guid.Parse(participantId), dateTime);
		return Result.Success();
	}

	public async Task<Result> RemoveParticipantFromEvent(string eventId, string participantId)
	{
		if (!await _eventsParticipantsRepository.IsExists(Guid.Parse(eventId), Guid.Parse(participantId)))
			return Result.Failure("Same registration doesn't exists");

		await _eventsParticipantsRepository.RemoveEventParticipant(Guid.Parse(eventId), Guid.Parse(participantId));
		return Result.Success();
	}

	public async Task<Result<Guid>> Update(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, string imageUrl)
	{
		if (!await _eventsRepository.IsExists(id))
			return Result.Failure<Guid>("Event with this id doesn't exists");

		var eventModel = EventModel.Create(id, title, description, eventDateTime, location, category, maxParticipants, imageUrl);

		if (eventModel.IsFailure)
			return Result.Failure<Guid>(eventModel.Error);

		return await _eventsRepository.Update(eventModel.Value);
	}

	public async Task<Result> Delete(Guid eventId)
	{
		if (!await _eventsRepository.IsExists(eventId))
			return Result.Failure("Event with this id doesn't exists");

		await _eventsRepository.Delete(eventId);
		return Result.Success();
	}
}
