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
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvents()
	{
		var events = await _eventsServices.Get();
		var response = _mapper.Map<IList<GetEventResponse>>(events);

		return Ok(response);
	}

	//[HttpGet(nameof(GetEventsForParticipant) + " /{id: Guid}")]
	//[Authorize(Policy = "UserOrAdmin")]
	//public async Task<IActionResult> GetEventsForParticipant(Guid particiapntId)
	//{
	//	var events = await _eventsServices.GetByParticipantId(particiapntId);
	//	var response = _mapper.Map<IList<GetEventResponse>>(events);

	//	return Ok(response);
	//}

	[HttpGet(nameof(GetEvent) + "/{id:Guid}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvent(Guid id)
	{
		var eventModel = await _eventsServices.Get(id);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<GetEventResponse>(eventModel.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetEventsByParticipant) + "/{id:Guid}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByParticipant(Guid id)
	{
		var eventModel = await _eventsServices.GetByParticipantId(id);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<IList<GetEventResponse>>(eventModel.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetEventByTitle) + "/{title}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventByTitle(string title)
	{
		var eventModel = await _eventsServices.GetByTitle(title);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<IList<GetEventResponse>>(eventModel.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetEventsByLocation) + "/{location}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByLocation(string location)
	{
		var eventModel = await _eventsServices.GetByLocation(location);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<IList<GetEventResponse>>(eventModel.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetEventsByCategory) + "/{category}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByCategory(string category)
	{
		var eventModel = await _eventsServices.GetByCategory(category);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<IList<GetEventResponse>>(eventModel.Value);

		return Ok(response);
	}

	[HttpGet(nameof(GetEventParticipants) + "/{id:Guid}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventParticipants(Guid id)
	{
		var users = await _eventsServices.GetEventParticipants(id);

		if (users.IsFailure)
			return BadRequest(users.Error);

		var response = _mapper.Map<IList<GetParticipantResponse>>(users.Value);

		return Ok(response);
	}

	[HttpPost($"{nameof(CreateEvent)}")]
	//[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventId = await _eventsServices.Create(request.Title, request.Description, request.EventDateTime, request.Location, request.Category, request.MaxParticipants, request.ImageUrl);

		if (eventId.IsFailure)
			return BadRequest(eventId.Error);

		return Ok(eventId.Value);
	}

	[HttpPost($"{nameof(RegistrationOnEvent)}")]
	//[Authorize(Policy = "UserOnly")]
	public async Task<IActionResult> RegistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		var result =  await _eventsServices.AddParticipantToEvent(request.EventId, request.ParticipantId);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}

	[HttpPost($"{nameof(UnregistrationOnEvent)}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> UnregistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		var result =  await _eventsServices.RemoveParticipantFromEvent(request.EventId, request.ParticipantId);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}

	[HttpPut(nameof(Update))]
	//[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Update([FromBody] UpdateEventRequest request)
	{
		var result =  await _eventsServices.Update(request.Id, request.Title, request.Description, request.EventDateTime, request.Location, request.Category, request.MaxParticipants, request.ImageUrl);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(result.Value);
	}

	[HttpDelete(nameof(Delete) + "/{id:Guid}")]
	//[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result =  await _eventsServices.Delete(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}
}
