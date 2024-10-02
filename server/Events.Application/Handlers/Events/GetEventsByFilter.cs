using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

using MapsterMapper;

using MediatR;
using Events.Application.Common.Cache;
using Events.Application.DTOs;

namespace Events.Application.Quries.Events;

public class GetEventsByFilterQuery(string? title = null,
							   string? location = null,
							   string? category = null,
							   Guid? participantId = null) : IRequest<IList<EventDto>>
{
	public string? Title { get; private set; } = title;
	public string? Location { get; private set; } = location;
	public string? Category { get; private set; } = category;
	public Guid? ParticipantId { get; private set; } = participantId;
}

public class GetEventsByFilterQueryHandler(IEventsRepository eventsRepository,
										   IMapper mapper,
										   IRedisCacheCheck redisCacheCheck) : IRequestHandler<GetEventsByFilterQuery, IList<EventDto>>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;
	private readonly IRedisCacheCheck _redisCacheCheck = redisCacheCheck;

	public async Task<IList<EventDto>> Handle(GetEventsByFilterQuery request, CancellationToken cancellationToken)
	{
		IList<Guid>? eventIds = null;

		if (request.ParticipantId.HasValue)
			eventIds = await _eventsRepository.GetIdsByParticipantId(request.ParticipantId.Value, cancellationToken);
		else if (!string.IsNullOrEmpty(request.Title))
			eventIds = await _eventsRepository.GetIdsByTitle(request.Title, cancellationToken);
		else if (!string.IsNullOrEmpty(request.Location))
			eventIds = await _eventsRepository.GetIdsByLocation(request.Location, cancellationToken);
		else if (!string.IsNullOrEmpty(request.Category))
			eventIds = await _eventsRepository.GetIdsByCategory(request.Category, cancellationToken);

		if (eventIds == null || eventIds.Count == 0)
			throw new NotFoundException($"Event(s) not found");

		IList<EventModel> eventModels;

		if (eventIds.Count > 1)
			eventModels = await _redisCacheCheck.CheckImagesInCache(eventIds, cancellationToken);
		else
		{
			var singleEventModel = await _redisCacheCheck.CheckImageInCache(eventIds[0], cancellationToken);
			eventModels = [singleEventModel];
		}

		return _mapper.Map<IList<EventDto>>(eventModels);
	}
}
