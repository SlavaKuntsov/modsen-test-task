using Events.API.Contracts;
using Events.Domain.Interfaces;
using Events.Domain.Models;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class EventsController : BaseController
{
	private readonly IEventRepository _eventRepository;
	private readonly IMapper _mapper;

	public EventsController(IEventRepository eventRepository, IMapper mapper)
	{
		_eventRepository = eventRepository;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> GetEvents()
	{
		var eventsModels = await _eventRepository.GetEvents();

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
