using Events.Domain.Interfaces.Repositories;
using MediatR;
using Events.Domain.Models;
using MapsterMapper;

namespace Events.Application.Handlers.Events;

public class CreateEventCommand(Guid id,
								string title,
								string description,
								DateTime eventDateTime,
								string location,
								string category,
								int maxParticipants,
								byte[] imageUrl) : IRequest<Guid>
{
	public Guid Id { get; private set; } = id;
	public string Title { get; private set; } = title;
	public string Description { get; private set; } = description;
	public DateTime EventDateTime { get; private set; } = eventDateTime;
	public string Location { get; private set; } = location;
	public string Category { get; private set; } = category;
	public int MaxParticipants { get; private set; } = maxParticipants;
	public byte[] Image { get; private set; } = imageUrl ?? [];
}

public class CreateEventCommandHandler(IEventsRepository eventsRepository, IMapper mapper) : IRequestHandler<CreateEventCommand, Guid>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
	{
		var eventModel = _mapper.Map<EventModel>(request);

		return await _eventsRepository.Create(eventModel, cancellationToken);
	}
}
