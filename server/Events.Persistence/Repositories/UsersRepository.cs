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

	public async Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive)
	{
		var entity = await _context.Admins.FindAsync(id);

		entity!.IsActiveAdmin = isActive;

		await _context.SaveChangesAsync();

		return _mapper.Map<AdminModel>(entity);
	}
}
