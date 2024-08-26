using Events.API.Contracts;
using Events.Domain.Interfaces;
using Events.Domain.Models;

using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

public class EventsController : BaseController
{
	private readonly IEventRepository _eventRepository;

	public EventsController(IEventRepository eventRepository)
	{
		_eventRepository = eventRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetEvents()
	{
		var response = await _eventRepository.GetEvents();

		return Ok(response);
	}

	[HttpPost]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventModel = EventModel.Create(Guid.NewGuid(), request.Title); 

		if(eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var eventId = await _eventRepository.Create(eventModel.Value);

		return Ok(eventId);
	}
}
