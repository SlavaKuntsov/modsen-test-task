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
			.Select(ep => ep.EventId)
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
			EventRegistrationDate = date
		};

		await _context.EventsParticipants.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task RemoveEventParticipant(Guid eventId, Guid participantId)
	{
		var entity = await _context.EventsParticipants
			.FirstOrDefaultAsync(e => e.EventId == eventId && e.ParticipantId == participantId);

		if (entity != null)
		{
			_context.EventsParticipants.Remove(entity);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> IsExists(Guid eventId, Guid participantId)
	{
		return await _context.EventsParticipants
			.AnyAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);
	}
}
