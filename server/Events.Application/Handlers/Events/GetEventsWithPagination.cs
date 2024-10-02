using Events.Application.DTOs;
using Events.Domain.Interfaces.Repositories;

using MapsterMapper;

using MediatR;

namespace Events.Application.Handlers.Events;

public class GetEventsWithPaginationQuery(int pageNumber = 1, int pageSize = 10) : IRequest<IList<EventDto>>
{
	public int PageNumber { get; set; } = pageNumber;
	public int PageSize { get; set; } = pageSize;
}

public class GetEventsWithPaginationQueryHandler(IEventsRepository eventsRepository, IMapper mapper) : IRequestHandler<GetEventsWithPaginationQuery, IList<EventDto>>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<IList<EventDto>> Handle(GetEventsWithPaginationQuery request, CancellationToken cancellationToken)
	{
		var eventModels = await _eventsRepository.GetEventsWithPagination(request.PageNumber, request.PageSize, cancellationToken);

		if (eventModels == null || !eventModels.Any())
			return [];

		return _mapper.Map<IList<EventDto>>(eventModels);
	}
}
