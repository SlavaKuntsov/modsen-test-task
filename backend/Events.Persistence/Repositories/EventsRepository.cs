using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Events.Domain.Interfaces;
using Events.Domain.Models;
using Events.Persistence.Entities;

using Mapster;

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

	public async Task<ICollection<EventModel>> Get()
	{
		ICollection<EventEntity> eventsEntities = await _context
			.Events
			.AsNoTracking()
			.ToListAsync();

		var eventsModels = _mapper.Map<ICollection<EventModel>>(eventsEntities);

		return eventsModels;
	}

	public async Task<Guid> Create(EventModel eventModel)
	{
		var eventEntity = _mapper.Map<EventEntity>(eventModel);

		await _context.Events.AddAsync(eventEntity);
		
		await _context.SaveChangesAsync();

		return eventEntity.Id;
	}
}
