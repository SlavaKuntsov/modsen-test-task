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
		//var eventModels = await _eventsServices.Get();

	    // изменил на получение событий без изображений, чтобы оптимизировать запросы
		var eventModels = await _eventsServices.Get();

		var responses = _mapper.Map<IList<GetEventResponse>>(eventModels);

		for (int i = 0; i < responses.Count; i++)
		{
			var eventModel = eventModels[i];
			if (eventModel.Image != null)
				responses[i].Image = Convert.ToBase64String(eventModel.Image);
			else
				responses[i].Image = "";
		}

		return Ok(responses);
	}

	[HttpGet(nameof(GetEvent) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvent(Guid id)
	{
		var eventModel = await _eventsServices.Get(id);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		var response = _mapper.Map<GetEventResponse>(eventModel.Value);

		if (eventModel.Value.Image != null)
			response.Image = Convert.ToBase64String(eventModel.Value.Image); 
		else
			response.Image = "";

		return Ok(response);
	}

	[HttpGet(nameof(GetEventsByParticipant) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByParticipant(Guid id)
	{
		var eventModels = await _eventsServices.GetByParticipantId(id);

		if (eventModels.IsFailure)
			return BadRequest(eventModels.Error);

		var responses = _mapper.Map<IList<GetEventResponse>>(eventModels.Value);

		for (int i = 0; i < responses.Count; i++)
		{
			var eventModel = eventModels.Value[i]; 
			if (eventModel.Image != null) 
				responses[i].Image = Convert.ToBase64String(eventModel.Image); 
			else
				responses[i].Image = "";
		}

		return Ok(responses);
	}

	[HttpGet(nameof(GetEventByTitle) + "/{title}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventByTitle(string title)
	{
		var eventModels = await _eventsServices.GetByTitle(title);

		if (eventModels.IsFailure)
			return BadRequest(eventModels.Error);

		var responses = _mapper.Map<IList<GetEventResponse>>(eventModels.Value);

		for (int i = 0; i < responses.Count; i++)
		{
			var eventModel = eventModels.Value[i]; 
			if (eventModel.Image != null) 
				responses[i].Image = Convert.ToBase64String(eventModel.Image);
			else
				responses[i].Image = "";
		}

		return Ok(responses);
	}

	[HttpGet(nameof(GetEventsByLocation) + "/{location}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByLocation(string location)
	{
		var eventModels = await _eventsServices.GetByLocation(location);

		if (eventModels.IsFailure)
			return BadRequest(eventModels.Error);

		var responses = _mapper.Map<IList<GetEventResponse>>(eventModels.Value);

		for (int i = 0; i < responses.Count; i++)
		{
			var eventModel = eventModels.Value[i]; 
			if (eventModel.Image != null) 
				responses[i].Image = Convert.ToBase64String(eventModel.Image); 
			else
				responses[i].Image = "";
		}

		return Ok(responses);
	}

	[HttpGet(nameof(GetEventsByCategory) + "/{category}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByCategory(string category)
	{
		var eventModels = await _eventsServices.GetByCategory(category);

		if (eventModels.IsFailure)
			return BadRequest(eventModels.Error);

		var responses = _mapper.Map<IList<GetEventResponse>>(eventModels.Value);

		for (int i = 0; i < responses.Count; i++)
		{
			var eventModel = eventModels.Value[i]; 
			if (eventModel.Image != null) 
				responses[i].Image = Convert.ToBase64String(eventModel.Image); 
			else
				responses[i].Image = "";
		}

		return Ok(responses);
	}

	[HttpGet(nameof(GetParticipantsByEvent) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetParticipantsByEvent(Guid id)
	{
		var users = await _eventsServices.GetEventParticipants(id);

		if (users.IsFailure)
			return BadRequest(users.Error);

		var response = _mapper.Map<IList<GetParticipantResponse>>(users.Value);

		return Ok(response);
	}

	[HttpPost($"{nameof(CreateEvent)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		var eventId = await _eventsServices.Create(
			request.Title,
			request.Description,
			request.EventDateTime,
			request.Location,
			request.Category,
			request.MaxParticipants,
			request.Image
		);

		if (eventId.IsFailure)
			return BadRequest(eventId.Error);

		return Ok(eventId.Value);
	}

	[HttpPost($"{nameof(RegistrationOnEvent)}")]
	[Authorize(Policy = "UserOnly")]
	public async Task<IActionResult> RegistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		var result =  await _eventsServices.AddParticipantToEvent(request.EventId, request.ParticipantId);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}

	[HttpPost($"{nameof(UnregistrationOnEvent)}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> UnregistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		var result =  await _eventsServices.RemoveParticipantFromEvent(request.EventId, request.ParticipantId);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}

	[HttpPut(nameof(Update))]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Update([FromBody] UpdateEventRequest request)
	{
		var result =  await _eventsServices.Update(request.Id, request.Title, request.Description, request.EventDateTime, request.Location, request.Category, request.MaxParticipants, request.Image);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok(result.Value);
	}

	[HttpDelete(nameof(Delete) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result =  await _eventsServices.Delete(id);

		if (result.IsFailure)
			return BadRequest(result.Error);

		return Ok();
	}
}
