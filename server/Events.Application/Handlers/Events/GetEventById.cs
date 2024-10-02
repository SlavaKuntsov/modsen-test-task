using Events.Application.DTOs;
using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;

using MapsterMapper;

using MediatR;

namespace Events.Application.Quries.Events;

public class GetEventByIdQuery(Guid id) : IRequest<EventDto>
{
	public Guid Id { get; private set; } = id;
}

public class GetEventByIdQueryHandler(IEventsRepository eventsRepository, IMapper mapper) : IRequestHandler<GetEventByIdQuery, EventDto>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
	{
		var eventModel = await _eventsRepository.GetById(request.Id, cancellationToken);

		if (eventModel == null)
			throw new NotFoundException($"Event with ID {request.Id} not found");

		return _mapper.Map<EventDto>(eventModel);
	}
}