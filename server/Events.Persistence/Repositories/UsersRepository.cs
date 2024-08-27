using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistence.Entities;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
	private readonly EventsDBContext _context;
	private readonly IMapper _mapper;

	public UsersRepository(EventsDBContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ParticipantModel> Get(string email)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email);

		if (participantEntitiy == null)
			return null;

		var eventsModels = _mapper.Map<ParticipantModel>(participantEntitiy);

		return eventsModels;
	}

	public async Task<ParticipantModel> Get(string email, string password)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);

		if (participantEntitiy == null)
			return null;

		var eventsModels = _mapper.Map<ParticipantModel>(participantEntitiy);

		return eventsModels;
	}

	public async Task<Guid> Create(ParticipantModel user)
	{
		var entity = _mapper.Map<ParticipantEntity>(user);

		await _context.Participants.AddAsync(entity);

		await _context.SaveChangesAsync();

		return entity.Id;
	}
}
