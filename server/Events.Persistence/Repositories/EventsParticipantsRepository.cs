using CSharpFunctionalExtensions;

using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Domain.Models.Users;
using Events.Persistence.Entities;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

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

	public async Task<IList<ParticipantModel>> GetParticipantsByEvent(Guid eventId, CancellationToken cancellationToken)
	{
		var participants = await _context.EventsParticipants
			.Where(ep => ep.EventId == eventId)
			.Select(ep => ep.Participant)
			.ToListAsync(cancellationToken);

		return _mapper.Map<IList<ParticipantModel>>(participants);
	}

	public async Task<IList<EventModel>> GetEventsByParticipant(Guid participantId, CancellationToken cancellationToken)
	{
		var events = await _context.EventsParticipants
			.Where(ep => ep.ParticipantId == participantId)
			.Select(ep => ep.Event)
			.ToListAsync(cancellationToken);

		return _mapper.Map<IList<EventModel>>(events);
	}

	public async Task AddEventParticipant(Guid eventId, Guid participantId, DateTime date, CancellationToken cancellationToken)
	{
		date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

		var entity = new EventParticipantEntity
		{
			EventId = eventId,
			ParticipantId = participantId,
			EventRegistrationDate = date,
		};

		if (entity != null)
		{
			using var transaction = _context.Database.BeginTransaction();

			try
			{
				await _context.EventsParticipants.AddAsync(entity, cancellationToken);

				var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
				if (eventEntity!.ParticipantsCount < eventEntity.MaxParticipants)
					eventEntity.ParticipantsCount++;

				await _context.SaveChangesAsync(cancellationToken);

				transaction.Commit();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				throw new InvalidOperationException($"An error occurred while adding event for participant and saving token: {ex.Message}", ex);
			}
		}
	}

	public async Task RemoveEventParticipant(Guid eventId, Guid participantId, CancellationToken cancellationToken)
	{
		var entity = await _context
			.EventsParticipants
			.FirstOrDefaultAsync(e => e.EventId == eventId && e.ParticipantId == participantId, cancellationToken);

		if (entity != null)
		{
			_context.EventsParticipants.Remove(entity);

			var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
			if (eventEntity!.ParticipantsCount != 0)
				eventEntity!.ParticipantsCount--;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task RemoveParticipantFromEvents(Guid participantId, IList<EventModel> events, CancellationToken cancellationToken)
	{
		var eventIds = events.Select(e => e.Id).ToList();

		var eventEntities = await _context.Events
			.Where(e => eventIds.Contains(e.Id))
			.ToListAsync(cancellationToken);

		foreach (var eventEntity in eventEntities)
		{
			var participantEvent = await _context
				.EventsParticipants
				.FirstOrDefaultAsync(e => e.EventId == eventEntity.Id && e.ParticipantId == participantId, cancellationToken);

			if (participantEvent != null)
			{
				if (eventEntity.ParticipantsCount > 0)
				{
					eventEntity.ParticipantsCount--;
				}
			}
		}

		await _context.SaveChangesAsync(cancellationToken);
	}


	public async Task<bool> IsExists(Guid eventId, Guid participantId, CancellationToken cancellationToken)
	{
		return await _context.EventsParticipants
			.AnyAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId, cancellationToken);
	}
}
