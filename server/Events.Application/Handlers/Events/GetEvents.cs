using Events.Application.DTOs;
using Events.Domain.Interfaces.Repositories;

using MapsterMapper;

using MediatR;

namespace Events.Application.Quries.Events;

public class GetEventsQuery : IRequest<IList<EventDto>>
{
}

public class GetEventsQueryHandler(IEventsRepository eventsRepository, IMapper mapper) : IRequestHandler<GetEventsQuery, IList<EventDto>>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<IList<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
	{
		var eventModels = await _eventsRepository.GetWithoutImage();

		if (eventModels == null || !eventModels.Any())
			return [];

		return _mapper.Map<IList<EventDto>>(eventModels);
	}
}