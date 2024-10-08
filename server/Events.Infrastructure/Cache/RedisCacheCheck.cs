﻿using System.Reflection;
using System.Threading;

using Events.Application.Common.Cache;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

using Mapster;

using MapsterMapper;

namespace Events.Infrastructure.Cache;

public class RedisCacheCheck : IRedisCacheCheck
{
	private readonly IEventsRepository _eventsRepository;
	private readonly IMapper _mapper;
	private readonly IRedisCache _redisCache;

	public RedisCacheCheck(IEventsRepository eventsRepository, IMapper mapper, IRedisCache redisCache)
	{
		_eventsRepository = eventsRepository;
		_mapper = mapper;
		_redisCache = redisCache;
	}

	public async Task<IList<EventModel>> CheckImagesInCache(IList<Guid> ids, CancellationToken cancellationToken)
	{
		IList<EventModel> result = [];

		foreach (var eventModelId in ids)
		{
			var cachedImage = await _redisCache.GetImage(eventModelId.ToString());
			EventModel? eventModel;

			if (cachedImage != null)
			{
				var modelWithoutImage = await _eventsRepository.GetByIdWithoutImage(eventModelId, cancellationToken);
				if (modelWithoutImage == null)
					break;

				EventModel newModel = new(modelWithoutImage.Id,
							  modelWithoutImage.Title,
							  modelWithoutImage.Description,
							  modelWithoutImage.EventDateTime,
							  modelWithoutImage.Location,
							  modelWithoutImage.Category,
							  modelWithoutImage.MaxParticipants,
							  modelWithoutImage.ParticipantsCount,
							  cachedImage);

				result.Add(newModel);
			}
			else
			{
				eventModel = await _eventsRepository.GetById(eventModelId, cancellationToken);
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
	public async Task<EventModel?> CheckImageInCache(Guid id, CancellationToken cancellationToken)
	{
		var cachedImage = await _redisCache.GetImage(id.ToString());
		EventModel? eventModel;
		if (cachedImage != null)
		{
			var modelWithoutImage = await _eventsRepository.GetByIdWithoutImage(id, cancellationToken);
			if (modelWithoutImage == null)
				return null;

			var newModel = modelWithoutImage.Adapt<EventModel>();
			newModel.SetImage(cachedImage);

			return newModel;
		}
		else
		{
			eventModel = await _eventsRepository.GetById(id, cancellationToken);
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
