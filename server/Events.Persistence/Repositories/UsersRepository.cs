using CSharpFunctionalExtensions;

using Events.Domain.Enums;
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

	public async Task<RefreshTokenModel?> GetRefreshToken(string refreshToken)
	{
		var entity = await _context
			.RefreshTokens
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.Token == refreshToken);

		if (entity == null)
			return null;

		return _mapper.Map<RefreshTokenModel>(entity);
	}

	// TODO - его логика добавлена в Create метод выше
	public async Task SaveRefreshToken(RefreshTokenModel refreshToken)
	{
		var entity = _mapper.Map<RefreshTokenEntity>(refreshToken);

		Console.WriteLine("---------------------");
		Console.WriteLine(entity.Id);
		Console.WriteLine(entity.Token);
		Console.WriteLine(entity.AdminId);
		Console.WriteLine(entity.UserId);

		_context.RefreshTokens.Add(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateRefreshToken(Guid userId, Role role, RefreshTokenModel newRefreshToken)
	{
		RefreshTokenEntity? existingToken = null;

		if (role == Role.Admin)
		{
			existingToken = await _context.RefreshTokens
				.FirstOrDefaultAsync(rt => rt.AdminId == userId);
		}
		else if (role == Role.User)
		{
			existingToken = await _context.RefreshTokens
				.FirstOrDefaultAsync(rt => rt.UserId == userId);
		}

		if (existingToken != null)
		{
			existingToken.Token = newRefreshToken.Token;
			existingToken.CreatedAt = DateTime.UtcNow;
			existingToken.ExpiresAt = DateTime.UtcNow.AddDays(30);

			_context.RefreshTokens.Update(existingToken);
			await _context.SaveChangesAsync();
		}
		else
			await SaveRefreshToken(newRefreshToken);
	}

	public async Task DeleteRefreshToken(string refreshToken)
	{
		var token = await _context
			.RefreshTokens
			.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

		if (token != null)
		{
			_context.RefreshTokens.Remove(token);
			await _context.SaveChangesAsync();
		}
	}
}
