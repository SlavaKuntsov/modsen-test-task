using CSharpFunctionalExtensions;

using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistence.Entities;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.Repositories;

public class EventsRepository : IEventsRepository
{
	private readonly EventsDBContext _context;
	private readonly IMapper _mapper;

	public EventsRepository(EventsDBContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<EventModel>> Get()
	{
		IList<EventEntity> eventsEntities = await _context
			.Events
			.AsNoTracking()
			.ToListAsync();

		return _mapper.Map<IList<EventModel>>(eventsEntities);
	}

	public async Task<EventModel?> Get(Guid id)
	{
		var eventEntitiy = await _context
			.Events
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (eventEntitiy == null)
			return null;

		return _mapper.Map<EventModel>(eventEntitiy);
	}

	public async Task<IList<EventModel>?> GetByParticipantId(Guid id)
	{
		var eventEntitiesId = await _context
		.EventsParticipants
		.AsNoTracking()
		.Where(p => p.ParticipantId == id)
		.Select(p => p.EventId)
		.ToListAsync();

		if (eventEntitiesId == null || eventEntitiesId.Count == 0)
			return null;

		var events = await _context
		.Events
		.Where(e => eventEntitiesId.Contains(e.Id))
		.ToListAsync();
		
		return _mapper.Map<IList<EventModel>>(events);
	}


	public async Task<IList<EventModel>?> GetByTitle(string title)
	{
		var eventEntities = await _context
			.Events
			.AsNoTracking()
			.Where(p => p.Title == title)
			.ToListAsync();

		if (eventEntities == null || eventEntities.Count == 0)
			return null;

		return _mapper.Map<IList<EventModel>>(eventEntities);
	}

	public async Task<IList<EventModel>?> GetByLocation(string location)
	{
		var eventEntities = await _context
			.Events
			.AsNoTracking()
			.Where(p => p.Location == location)
			.ToListAsync();

		if (eventEntities == null || eventEntities.Count == 0)
			return null;

		return _mapper.Map<IList<EventModel>>(eventEntities);
	}

	public async Task<IList<EventModel>?> GetByCategory(string category)
	{
		var eventEntities = await _context
			.Events
			.AsNoTracking()
			.Where(p => p.Category == category)
			.ToListAsync();

		if (eventEntities == null || eventEntities.Count == 0)
			return null;

		return _mapper.Map<IList<EventModel>>(eventEntities);
	}

	//public async Task<EventModel?> Get<T>(T value, Func<EventModel, bool> predicate)
	//{
	//	var eventEntity = await _context
	//	.Events
	//	.AsNoTracking()
	//	.FirstOrDefaultAsync(e => predicate(e));

	//	if (eventEntity == null)
	//		return null;

	//	var model = _mapper.Map<EventModel>(eventEntity);
	//	return model;
	//}

	public async Task<Guid> Create(EventModel eventModel)
	{
		var entity = _mapper.Map<EventEntity>(eventModel);
		await _context.Events.AddAsync(entity);

		await _context.SaveChangesAsync();
		return entity.Id;
	}

	public async Task<Guid> Update(EventModel eventModel)
	{
		var entity = await _context.Events.FindAsync(eventModel.Id);

		entity!.Title = eventModel.Title;
		entity.Description = eventModel.Description;
		entity.EventDateTime = eventModel.EventDateTime;
		entity.Location = eventModel.Location;
		entity.Category = eventModel.Category;
		entity.MaxParticipants = eventModel.MaxParticipants;
		entity.Image = eventModel.Image;

		await _context.SaveChangesAsync();

		return entity.Id;
	}

	public async Task Delete(Guid eventId)
	{
		var entity = await _context.Events.FindAsync(eventId);

		_context.Events.Remove(entity!);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> IsExists(Guid eventId)
	{
		return await _context.Events
			.AnyAsync(ep => ep.Id == eventId);
	}
}
