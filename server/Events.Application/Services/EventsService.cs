using System.Data.SqlTypes;
using System.Diagnostics;

using CSharpFunctionalExtensions;

using Events.Application.Cache;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Application.Services;

public class EventsService : IEventsServices
{
	private readonly IEventsRepository _eventsRepository;
	private readonly IEventsParticipantsRepository _eventsParticipantsRepository;
	private readonly IRedisCache _redisCache;

	public EventsService(
		IEventsRepository eventsRepository,
		IEventsParticipantsRepository eventsParticipantsRepository,
		IRedisCache redisCache)
	{
		_eventsRepository = eventsRepository;
		_eventsParticipantsRepository = eventsParticipantsRepository;
		_redisCache = redisCache;
	}

	public async Task<IList<EventModel>> Get()
	{
		var existEventsId = await _eventsRepository.GetIds();

		return await CheckImageInCache(existEventsId);
	}

	public async Task<Result<EventModel>> Get(Guid id)
	{
		//var existEvent = await _eventsRepository.GetById(id);

		var existEvent = await CheckImageInCache(id);

		if (existEvent == null)
			return Result.Failure<EventModel>("Event with this id doesn't exists");

		return existEvent;
	}

	public async Task<Result<IList<EventModel>>> GetByParticipantId(Guid id)
	{
		var existEventsId = await _eventsRepository.GetIdsByParticipantId(id);

		return Result.Success(await CheckImageInCache(existEventsId));

		//if (existEvents == null)
		//	//return Result.Failure<IList<EventModel>>("Events for this participant doesn't exists");
		//	return Result.Success<IList<EventModel>>([]);

		//return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByTitle(string title)
	{
		var existEventsId = await _eventsRepository.GetIdsByTitle(title);

		return Result.Success(await CheckImageInCache(existEventsId));

		//var existEvents = await _eventsRepository.GetByTitle(title);

		//if (existEvents == null)
		//	return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		//return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByLocation(string location)
	{
		var existEventsId = await _eventsRepository.GetIdsByLocation(location);


		return Result.Success(await CheckImageInCache(existEventsId));
		//var existEvents = await _eventsRepository.GetByLocation(location);

		//if (existEvents == null)
		//	return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		//return Result.Success(existEvents);
	}

	public async Task<Result<IList<EventModel>>> GetByCategory(string category)
	{
		var existEventsId = await _eventsRepository.GetIdsByCategory(category);

		return Result.Success(await CheckImageInCache(existEventsId));

		//var existEvents = await _eventsRepository.GetByCategory(category);

		//if (existEvents == null)
		//	return Result.Failure<IList<EventModel>>("Events with this title doesn't exists");

		//return Result.Success(existEvents);
	}

	public async Task<Result<IList<ParticipantModel>>> GetEventParticipants(Guid eventId)
	{
		var particants = await _eventsParticipantsRepository.GetParticipantsByEvent(eventId);

		if (particants == null)
			return Result.Failure<IList<ParticipantModel>>("Particants not found");

		return Result.Success(particants);
	}

	public async Task<Result<Guid>> Create(string title, string description, string eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl)
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

	public async Task<Result<Guid>> Update(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl)
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

	private async Task<IList<EventModel>> CheckImageInCache(IList<Guid> ids)
	{
		IList<EventModel> result = [];

		foreach (var eventModelId in ids)
		{
			var cachedImage = await _redisCache.GetImage(eventModelId.ToString());

			EventModel? eventModel;

			if (cachedImage != null)
			{
				var modelWithoutImage = await _eventsRepository.GetByIdWithoutImage(eventModelId);
				if (modelWithoutImage == null)
					break;

				var newModel = EventModel.Create(modelWithoutImage, cachedImage);

				if (newModel.IsFailure)
					break;

				result.Add(newModel.Value);
			}
			else
			{
				eventModel = await _eventsRepository.GetById(eventModelId);

				if (eventModel != null && eventModel.Image != null)
				{
					await _redisCache.SetImage(eventModelId.ToString(), eventModel.Image, TimeSpan.FromHours(1));
				}
				else
					break;

				result.Add(eventModel!);
			}
		}
		return result;
	}

	private async Task<EventModel?> CheckImageInCache(Guid id)
	{
		var cachedImage = await _redisCache.GetImage(id.ToString());

		EventModel? eventModel;

		if (cachedImage != null)
		{
			var modelWithoutImage = await _eventsRepository.GetByIdWithoutImage(id);
			if (modelWithoutImage == null)
				return null;

			var newModel = EventModel.Create(modelWithoutImage, cachedImage);

			if (newModel.IsFailure)
				return null;

			return newModel.Value;
		}
		else
		{
			eventModel = await _eventsRepository.GetById(id);

			if (eventModel != null && eventModel.Image != null)
			{
				await _redisCache.SetImage(id.ToString(), eventModel.Image, TimeSpan.FromHours(1));
			}
			else
				return null;

			return eventModel;
		}
	}
}
