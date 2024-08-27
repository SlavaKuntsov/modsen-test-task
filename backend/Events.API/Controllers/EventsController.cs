using Events.API.Contracts.Events;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class EventsController : BaseController
{
	private readonly IEventsServices _eventsServices;
	private readonly IMapper _mapper;

	public EventsController(IEventsServices eventsServices, IMapper mapper)
	{
		_eventsServices = eventsServices;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> GetEvents()
	{
		var events = await _eventsServices.Get();
		var response = _mapper.Map<ICollection<GetEventResponse>>(events);

		return Ok(response);
	}

	[HttpPost]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventModel = EventModel.Create(Guid.NewGuid(), request.Title);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		Guid eventId = await _eventsServices.Create(eventModel.Value);

		return Ok(eventId);
	}
}
