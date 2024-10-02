using CSharpFunctionalExtensions;

using Events.Application.Exceptions;
using Events.Domain.Interfaces;
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

	public async Task<IList<T>> Get<T>(CancellationToken cancellationToken) where T : IUser
	{
		if (typeof(T) == typeof(ParticipantModel))
		{
			var participantEntities = await _context.Participants
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return _mapper.Map<IList<ParticipantModel>>(participantEntities) as IList<T>
				?? throw new InvalidOperationException("Mapping error for ParticipantModel.");
		}
		else if (typeof(T) == typeof(AdminModel))
		{
			var adminEntities = await _context.Admins
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return _mapper.Map<IList<AdminModel>>(adminEntities) as IList<T>
				?? throw new InvalidOperationException("Mapping error for AdminModel.");
		}

		throw new InvalidOperationException("The entity was not found.");
	}


	public async Task<T?> Get<T>(Guid id, CancellationToken cancellationToken) where T : IUser
	{
		if (typeof(T) == typeof(ParticipantModel))
		{
			var participantEntity = await _context.Participants
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

			return participantEntity != null ? _mapper.Map<T>(participantEntity) : default;
		}
		else if (typeof(T) == typeof(AdminModel))
		{
			var adminEntity = await _context.Admins
				.AsNoTracking()
				.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

			return adminEntity != null ? _mapper.Map<T>(adminEntity) : default;
		}

		throw new InvalidOperationException("The entity was not found.");
	}

	public async Task<IList<AdminModel>> GetAdmins(CancellationToken cancellationToken)
	{
		var adminEntities = await _context
			.Admins
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		return _mapper.Map<IList<AdminModel>>(adminEntities);
	}

	public async Task<T?> Get<T>(string email, CancellationToken cancellationToken) where T : IUser
	{
		if (typeof(T) == typeof(ParticipantModel))
		{
			var participantEntity = await _context.Participants
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Email == email, cancellationToken);

			return participantEntity != null ? _mapper.Map<T>(participantEntity) : default;
		}
		else if (typeof(T) == typeof(AdminModel))
		{
			var adminEntity = await _context.Admins
				.AsNoTracking()
				.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);

			return adminEntity != null ? _mapper.Map<T>(adminEntity) : default;
		}

		throw new InvalidOperationException("The entity was not found.");
	}

	public async Task<ParticipantModel?> Get(string email, string password, CancellationToken cancellationToken)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email && p.Password == password, cancellationToken);

		if (participantEntitiy == null)
			return null;

		return _mapper.Map<ParticipantModel>(participantEntitiy);
	}

	public async Task<Result<Guid>> Create<T>(T user, RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken) where T : IUser
	{
		var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshTokenModel);

		using var transaction = _context.Database.BeginTransaction();

		try
		{
			if (typeof(T) == typeof(ParticipantModel))
			{
				var userEntity = _mapper.Map<ParticipantEntity>(user as ParticipantModel);
				await _context.Participants.AddAsync(userEntity, cancellationToken);
				await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);

				transaction.Commit();

				return userEntity.Id;
			}
			else if (typeof(T) == typeof(AdminModel))
			{
				var userEntity = _mapper.Map<AdminEntity>(user as AdminModel);
				var adminCount = await _context.Admins.CountAsync(cancellationToken);

				userEntity.IsActiveAdmin = adminCount == 0; // true, если это первая запись

				await _context.Admins.AddAsync(userEntity, cancellationToken);
				await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);

				transaction.Commit();

				return userEntity.Id;
			}

			throw new InvalidOperationException("The user type is not supported.");
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync(cancellationToken);
			throw new InvalidOperationException($"An error occurred while creating user and saving token: {ex.Message}", ex);
		}
	}

	public async Task<ParticipantModel> Update(ParticipantModel particantModel, CancellationToken cancellationToken)
	{
		var entity = await _context.Participants.FindAsync(particantModel.Id, cancellationToken);

		entity!.FirstName = particantModel.FirstName;
		entity!.LastName = particantModel.LastName;
		entity!.DateOfBirth = particantModel.DateOfBirth;

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ParticipantModel>(entity);
	}

	public async Task Delete<T>(Guid eventId, CancellationToken cancellationToken)
	{
		if (typeof(T) == typeof(ParticipantModel))
		{
			var entity = await _context.Participants.FindAsync(eventId, cancellationToken, cancellationToken);

			_context.Participants.Remove(entity!);
			await _context.SaveChangesAsync(cancellationToken);
		}
		else if (typeof(T) == typeof(AdminModel))
		{
			var adminCount = await _context.Admins.CountAsync(cancellationToken);

			if (adminCount == 1)
				throw new DeleteException("Сannot delete the last Admin");

			var entity = await _context.Admins.FindAsync(eventId, cancellationToken);

			_context.Admins.Remove(entity!);
			await _context.SaveChangesAsync(cancellationToken);
		}

		throw new InvalidOperationException("The entity was not found.");
	}

	public async Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive, CancellationToken cancellationToken)
	{
		var entity = await _context.Admins.FindAsync(id, cancellationToken);

		entity!.IsActiveAdmin = isActive;

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<AdminModel>(entity);
	}

	public async Task<bool> IsExists(Guid eventId, CancellationToken cancellationToken)
	{
		return await _context.Participants
			.AnyAsync(ep => ep.Id == eventId, cancellationToken);
	}
}
