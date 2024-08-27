using Events.API.Contracts.Events;
using Events.Domain.Interfaces;
using Events.Domain.Models;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class EventsController : BaseController
{
	private readonly IEventsRepository _eventRepository;
	private readonly IMapper _mapper;

	public EventsController(IEventsRepository eventRepository, IMapper mapper)
	{
		_eventRepository = eventRepository;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> GetEvents()
	{
		var eventsModels = await _eventRepository.Get();

		var response = _mapper.Map<ICollection<GetEventResponse>>(eventsModels);

		return Ok(response);
	}

	[HttpPost]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventModel = EventModel.Create(Guid.NewGuid(), request.Title);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		Guid eventId = await _eventRepository.Create(eventModel.Value);

		return Ok(eventId);
	}
}
