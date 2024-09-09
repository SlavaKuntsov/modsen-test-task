﻿using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Domain.Models.Users;
using Events.Persistence.Entities;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Events.Persistence.Repositories;

public class EventsParticipantsRepository : IEventsParticipantsRepository
{
	private readonly EventsDBContext _context;
	private readonly IMapper _mapper;

	public EventsParticipantsRepository(EventsDBContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<ParticipantModel>> GetParticipantsByEvent(Guid eventId)
	{
		var participants = await _context.EventsParticipants
			.Where(ep => ep.EventId == eventId)
			.Select(ep => ep.Participant)
			.ToListAsync();

		return _mapper.Map<IList<ParticipantModel>>(participants);
	}

	// TODO - нет по ТЗ
	public async Task<IList<EventModel>> GetEventsByParticipant(Guid participantId)
	{
		var events = await _context.EventsParticipants
			.Where(ep => ep.ParticipantId == participantId)
			.Select(ep => ep.Event)
			.ToListAsync();

		return _mapper.Map<IList<EventModel>>(events);
	}

	public async Task AddEventParticipant(Guid eventId, Guid participantId, DateTime date)
	{
		date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

		var entity = new EventParticipantEntity
		{
			EventId = eventId,
			ParticipantId = participantId,
			EventRegistrationDate = date,
		};

		await _context.EventsParticipants.AddAsync(entity);

		var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
		if (eventEntity!.ParticipantsCount < eventEntity.MaxParticipants)
			eventEntity!.ParticipantsCount++;

		await _context.SaveChangesAsync();
	}

	public async Task RemoveEventParticipant(Guid eventId, Guid participantId)
	{
		var entity = await _context.EventsParticipants
			.FirstOrDefaultAsync(e => e.EventId == eventId && e.ParticipantId == participantId);

		if (entity != null)
		{
			_context.EventsParticipants.Remove(entity);

			var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
			if (eventEntity!.ParticipantsCount != 0)
				eventEntity!.ParticipantsCount--;

			await _context.SaveChangesAsync();
		}
	}

	public async Task RemoveParticipantFromEvents(Guid participantId, IList<EventModel> events)
	{
		// Извлекаем EventId из модели
		var eventIds = events.Select(e => e.Id).ToList();

		// Загружаем связанные EventEntity напрямую из контекста
		var eventEntities = await _context.Events
		.Where(e => eventIds.Contains(e.Id))
		.ToListAsync();

		foreach (var eventEntity in eventEntities)
		{
			// Ищем участие участника в событии
			var participantEvent = await _context.EventsParticipants
			.FirstOrDefaultAsync(e => e.EventId == eventEntity.Id && e.ParticipantId == participantId);

			if (participantEvent != null)
			{
				// Удаляем участие
				//_context.EventsParticipants.Remove(participantEvent);

				// Уменьшаем счетчик участников
				if (eventEntity.ParticipantsCount > 0)
				{
					eventEntity.ParticipantsCount--;
				}
			}
		}

		// Сохраняем все изменения
		await _context.SaveChangesAsync();
	}


	public async Task<bool> IsExists(Guid eventId, Guid participantId)
	{
		return await _context.EventsParticipants
			.AnyAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);
	}
}
