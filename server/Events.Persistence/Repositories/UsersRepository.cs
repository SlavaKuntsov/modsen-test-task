using CSharpFunctionalExtensions;

using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Domain.Models.Users;
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

	public async Task<ParticipantModel?> Get(Guid id)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<ParticipantModel>(participantEntitiy);
	}

	public async Task<AdminModel?> GetAdmin(Guid id)
	{
		var participantEntitiy = await _context
			.Admins
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<AdminModel>(participantEntitiy);
	}

	public async Task<IList<AdminModel>> GetAdmins()
	{
		var adminEntities = await _context
			.Admins
			.AsNoTracking()
			.ToListAsync();

		return _mapper.Map<IList<AdminModel>>(adminEntities);
	}

	public async Task<ParticipantModel?> Get(string email)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<ParticipantModel>(participantEntitiy);
	}

	public async Task<AdminModel?> GetAdmin(string email)
	{
		var participantEntitiy = await _context
			.Admins
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<AdminModel>(participantEntitiy);
	}

	public async Task<ParticipantModel?> Get(string email, string password)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<ParticipantModel>(participantEntitiy);
	}

	public async Task<Result<Guid>> Create(ParticipantModel user, RefreshTokenModel refreshTokenModel)
	{
		var userEntity = _mapper.Map<ParticipantEntity>(user);
		var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshTokenModel);

		using var transaction = _context.Database.BeginTransaction();

		try
		{
			await _context.Participants.AddAsync(userEntity);
			await _context.RefreshTokens.AddAsync(refreshTokenEntity);
			await _context.SaveChangesAsync();

			transaction.Commit();

			return userEntity.Id;
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return Result.Failure<Guid>($"An error occurred while creating user and saving token: {ex.Message}");
		}
	}

	public async Task<Result<Guid>> Create(AdminModel user, RefreshTokenModel refreshTokenModel)
	{
		var userEntity = _mapper.Map<AdminEntity>(user);
		var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshTokenModel);

		using var transaction = _context.Database.BeginTransaction();

		try
		{
			await _context.Admins.AddAsync(userEntity);
			await _context.RefreshTokens.AddAsync(refreshTokenEntity);
			await _context.SaveChangesAsync();

			transaction.Commit();

			return userEntity.Id;
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return Result.Failure<Guid>($"An error occurred while creating admin and saving token: {ex.Message}");
		}
	}

	public async Task<ParticipantModel> Update(ParticipantModel particantModel)
	{
		var entity = await _context.Participants.FindAsync(particantModel.Id);

		//entity!.Email = particantModel.Email;
		//entity!.Password = particantModel.Password;
		entity!.FirstName = particantModel.FirstName;
		entity!.LastName = particantModel.LastName;
		entity!.DateOfBirth = particantModel.DateOfBirth;

		await _context.SaveChangesAsync();

		return _mapper.Map<ParticipantModel>(entity);
	}

	public async Task Delete(Guid eventId)
	{
		var entity = await _context.Participants.FindAsync(eventId);

		_context.Participants.Remove(entity!);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAdmin(Guid eventId)
	{
		var entity = await _context.Admins.FindAsync(eventId);

		_context.Admins.Remove(entity!);
		await _context.SaveChangesAsync();
	}

	public async Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive)
	{
		var entity = await _context.Admins.FindAsync(id);

		entity!.IsActiveAdmin = isActive;

		await _context.SaveChangesAsync();

		return _mapper.Map<AdminModel>(entity);
	}

	public async Task<bool> IsExists(Guid eventId)
	{
		return await _context.Participants
			.AnyAsync(ep => ep.Id == eventId);
	}
}
