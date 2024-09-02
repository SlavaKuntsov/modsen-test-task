using Events.API.Contracts.Events;
using Events.Domain.Interfaces.Services;

using MapsterMapper;

using Microsoft.AspNetCore.Authorization;
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

	[HttpGet($"{nameof(GetEvents)}")]
	[Authorize(Policy = "UserOnly")]
	public async Task<IActionResult> GetEvents()
	{

		var events = await _eventsServices.Get();
		var response = _mapper.Map<IList<GetEventResponse>>(events);

		return Ok(response);
	}

	[HttpGet($"{nameof(GetEventsAdmin)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetEventsAdmin()
	{
		var events = await _eventsServices.Get();
		var response = _mapper.Map<IList<GetEventResponse>>(events);

		return Ok(response);
	}

	[HttpGet(nameof(GetEvent) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvent(Guid id)
	{
		var eventModel = await _eventsServices.Get(id);

		var response = _mapper.Map<GetEventResponse>(eventModel.Value);

		return Ok(response);
	}

	[HttpPost($"{nameof(CreateEvent)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventId = await _eventsServices.Create(Guid.NewGuid(), request.Title, request.Description, request.EventDateTime, request.Location, request.Category, request.MaxParticipants, request.ImageUrl);

		if (eventId.IsFailure)
			return BadRequest(eventId.Error);

		return Ok(eventId.Value);
	}
}
