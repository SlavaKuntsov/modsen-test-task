using System.Diagnostics;

using Events.API.Contracts.Events;
using Events.Domain.Interfaces.Services;
using Events.Domain.Models;

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
		//if (!User.Identity.IsAuthenticated)
		//{
		//	return Unauthorized("User is not authenticated");
		//}
		var claims = User.Claims.ToList();
		// Проверка, действительно ли пользователь авторизован
		Debug.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
		// Вывод всех claims
		foreach (var claim in claims)
		{
			Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
		}

		var events = await _eventsServices.Get();
		var response = _mapper.Map<IList<GetEventResponse>>(events);

		return Ok(response);
	}

	[HttpGet($"{nameof(GetEventsAdmin)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> GetEventsAdmin()
	{
		//if (!User.Identity.IsAuthenticated)
		//{
		//	return Unauthorized("User is not authenticated");
		//}
		var claims = User.Claims.ToList();
		// Проверка, действительно ли пользователь авторизован
		Debug.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
		// Вывод всех claims
		foreach (var claim in claims)
		{
			Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
		}

		var events = await _eventsServices.Get();
		var response = _mapper.Map<IList<GetEventResponse>>(events);

		return Ok(response);
	}

	[HttpPost]
	[Authorize(Policy = "AdminOnly")]
	public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
	{
		if (!User.Identity.IsAuthenticated)
		{
			return Unauthorized("User is not authenticated");
		}
		var claims = User.Claims.ToList();
		// Проверка, действительно ли пользователь авторизован
		Debug.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
		// Вывод всех claims
		foreach (var claim in claims)
		{
			Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
		}

		var eventModel = EventModel.Create(Guid.NewGuid(), request.Title);

		if (eventModel.IsFailure)
			return BadRequest(eventModel.Error);

		Guid eventId = await _eventsServices.Create(eventModel.Value);

		return Ok(eventId);
	}
}
