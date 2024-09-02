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

		var eventsModels = _mapper.Map<IList<EventModel>>(eventsEntities);

		return eventsModels;
	}

	public async Task<EventModel?> Get(Guid id)
	{
		var eventEntitiy = await _context
			.Events
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (eventEntitiy == null)
			return null;

		var model = _mapper.Map<EventModel>(eventEntitiy);

		return model;
	}

	public async Task<Guid> Create(EventModel eventModel)
	{
		var entity = _mapper.Map<EventEntity>(eventModel);
		await _context.Events.AddAsync(entity);

		await _context.SaveChangesAsync();
		return entity.Id;
	}
}
