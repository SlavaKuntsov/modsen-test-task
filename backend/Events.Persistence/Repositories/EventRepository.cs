using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Events.Domain.Interfaces;
using Events.Domain.Models;
using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.Repositories;

public class EventRepository : IEventRepository
{
	private readonly EventsDBContext _context;

	public EventRepository(EventsDBContext context) => _context = context;

	public async Task<ICollection<string>> GetEvents()
	{
		var events = await _context
			.Events
			.AsNoTracking()
			.Select(e => e.Title)
			.ToListAsync();

		return events;
	}

	public async Task<Guid> Create(EventModel eventModel)
	{
		EventEntity eventEntity = new()
		{
			Id = eventModel.Id,
			Title = eventModel.Title,
		};

		await _context.Events.AddAsync(eventEntity);
		
		await _context.SaveChangesAsync();

		return eventEntity.Id;
	}
}
