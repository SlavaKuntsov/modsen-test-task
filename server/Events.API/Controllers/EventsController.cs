using System.Globalization;

using Events.API.Contracts.Events;
using Events.Application.Handlers.Events;
using Events.Application.Quries.Events;
using Events.Domain.Constants;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public EventsController(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	[HttpGet($"{nameof(GetEvents)}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvents()
	{
		var eventDtos = await _mediator.Send(new GetEventsQuery());

		return Ok(eventDtos);
	}

	[HttpGet($"{nameof(GetEventsWithPagination)}")]
	//[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var eventDtos = await _mediator.Send(new GetEventsWithPaginationQuery(pageNumber, pageSize));

		return Ok(eventDtos);
	}

	[HttpGet(nameof(GetEvent) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEvent(Guid id)
	{
		var eventDto = await _mediator.Send(new GetEventByIdQuery(id));

		return Ok(eventDto);
	}

	[HttpGet(nameof(GetEventsByParticipant) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByParticipant(Guid id)
	{
		var eventDtos = await _mediator.Send(new GetEventsByFilterQuery(participantId: id));

		return Ok(eventDtos);
	}

	[HttpGet(nameof(GetEventByTitle) + "/{title}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventByTitle(string title)
	{
		var eventDtos = await _mediator.Send(new GetEventsByFilterQuery(title: title));

		return Ok(eventDtos);
	}

	[HttpGet(nameof(GetEventsByLocation) + "/{location}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByLocation(string location)
	{
		var eventDtos = await _mediator.Send(new GetEventsByFilterQuery(location: location));

		return Ok(eventDtos);
	}

	[HttpGet(nameof(GetEventsByCategory) + "/{category}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetEventsByCategory(string category)
	{
		var eventDtos = await _mediator.Send(new GetEventsByFilterQuery(category : category));

		return Ok(eventDtos);
	}

	[HttpGet(nameof(GetParticipantsByEvent) + "/{id:Guid}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> GetParticipantsByEvent(Guid id)
	{
		var users = await _mediator.Send(new GetEventParticipantsQuery(id));

		return Ok(users);
	}

	[HttpPost($"{nameof(CreateEvent)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		if (!DateTime.TryParseExact(request.EventDateTime, DateTimeConst.DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
			return BadRequest("Invalid date format.");

		var eventId = await _mediator.Send(new CreateEventCommand(
			Guid.NewGuid(),
			request.Title,
			request.Description,
			parsedDateTime,
			request.Location,
			request.Category,
			request.MaxParticipants,
			request.Image));

		return Ok(eventId);
	}

	[HttpPost($"{nameof(RegistrationOnEvent)}")]
	[Authorize(Policy = "UserOnly")]
	public async Task<IActionResult> RegistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		await _mediator.Send(new AddParticipantToEventCommand(request.EventId, request.ParticipantId));

		return Ok();
	}

	[HttpPost($"{nameof(UnregistrationOnEvent)}")]
	[Authorize(Policy = "UserOrAdmin")]
	public async Task<IActionResult> UnregistrationOnEvent([FromBody] CreateRegistrationOnEventRequest request)
	{
		await _mediator.Send(new RemoveParticipantFromEventCommand(request.EventId, request.ParticipantId));

		return Ok();
	}

	[HttpPut(nameof(Update))]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Update([FromBody] UpdateEventRequest request)
	{
		if (!DateTime.TryParseExact(request.EventDateTime, DateTimeConst.DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
			return BadRequest("Invalid date format.");

		var eventId = await _mediator.Send(new UpdateEventCommand(
			request.Id,
			request.Title,
			request.Description,
			parsedDateTime,
			request.Location,
			request.Category,
			request.MaxParticipants,
			request.ParticipantsCount,
			request.Image));

		return Ok(eventId);
	}

	[HttpDelete(nameof(Delete) + "/{id:Guid}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _mediator.Send(new DeleteEventCommand(id));

		return Ok();
	}
}
