using Events.Domain.Interfaces.Repositories;
using MediatR;
using Events.Domain.Models;

namespace Events.Application.Handlers.Events;

public class CreateEventCommand(string title,
								string description,
								DateTime eventDateTime,
								string location,
								string category,
								int maxParticipants,
								byte[] imageUrl) : IRequest<Guid>
{
	public string Title { get; private set; } = title;
	public string Description { get; private set; } = description;
	public DateTime EventDateTime { get; private set; } = eventDateTime;
	public string Location { get; private set; } = location;
	public string Category { get; private set; } = category;
	public int MaxParticipants { get; private set; } = maxParticipants;
	public byte[] Image { get; private set; } = imageUrl ?? [];
}

public class CreateEventCommandHandler(IEventsRepository eventsRepository) : IRequestHandler<CreateEventCommand, Guid>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;

	public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
	{
		EventModel eventModel = new(
			Guid.NewGuid(),
			request.Title,
			request.Description,
			request.EventDateTime, 
            request.Location,
			request.Category,
			request.MaxParticipants,
			request.Image);

		return await _eventsRepository.Create(eventModel);
	}
}
