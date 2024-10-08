﻿using Events.Application.Exceptions;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

using MapsterMapper;

using MediatR;

namespace Events.Application.Handlers.Events;

public class UpdateEventCommand(Guid id,
								string title,
								string description,
								DateTime eventDateTime,
								string location,
								string category,
								int maxParticipants,
								int participantsCount,
								byte[] imageUrl) : IRequest<Guid>
{
	public Guid Id { get; private set; } = id;
	public string Title { get; private set; } = title;
	public string Description { get; private set; } = description;
	public DateTime EventDateTime { get; private set; } = eventDateTime;
	public string Location { get; private set; } = location;
	public string Category { get; private set; } = category;
	public int MaxParticipants { get; private set; } = maxParticipants;
	public int ParticipantsCount { get; private set; } = participantsCount;
	public byte[] Image { get; private set; } = imageUrl ?? [];
}

public class UpdateEventCommandHandler(IEventsRepository eventsRepository, IMapper mapper) : IRequestHandler<UpdateEventCommand, Guid>
{
	private readonly IEventsRepository _eventsRepository = eventsRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<Guid> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
	{
		if (!await _eventsRepository.IsExists(request.Id, cancellationToken))
			throw new RegistrationExistsException("Event with this id doesn't exists");

		var eventModel = _mapper.Map<EventModel>(request);

		return await _eventsRepository.Update(eventModel, cancellationToken);
	}
}
