﻿using Events.Application.Auth;
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

	public async Task<ParticipantModel?> Get(Guid id)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);

		if (participantEntitiy == null)
			return null;

		var model = _mapper.Map<ParticipantModel>(participantEntitiy);

		return model;
	}

	public async Task<ParticipantModel?> Get(string email)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email);

		if (participantEntitiy == null)
			return null;

		var model = _mapper.Map<ParticipantModel>(participantEntitiy);

		return model;
	}

	public async Task<ParticipantModel?> Get(string email, string password)
	{
		var participantEntitiy = await _context
			.Participants
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);

		if (participantEntitiy == null)
			return null;

		var model = _mapper.Map<ParticipantModel>(participantEntitiy);

		return model;
	}

	public async Task<Guid> Create(ParticipantModel user)
	{
		var entity = _mapper.Map<ParticipantEntity>(user);

		await _context.Participants.AddAsync(entity);
		await _context.SaveChangesAsync();

		return entity.Id;
	}

	public async Task<RefreshTokenModel?> GetRefreshToken(string refreshToken)
	{
		var entity = await _context
			.RefreshTokens
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.Token == refreshToken);

		if (entity == null)
			return null;

		var model = _mapper.Map<RefreshTokenModel>(entity);

		return model;
	}

	// TODO - где применять??
	public async Task SaveRefreshToken(RefreshTokenModel refreshToken)
	{
		var entity = _mapper.Map<RefreshTokenEntity>(refreshToken);

		_context.RefreshTokens.Add(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateRefreshToken(Guid userId, string newRefreshToken)
	{
		// Получаем существующий refresh token для данного пользователя
		var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);

		if (existingToken != null)
		{
			// Обновляем токен и время его создания
			existingToken.Token = newRefreshToken;
			existingToken.CreatedAt = DateTime.UtcNow;
			existingToken.ExpiresAt = DateTime.UtcNow.AddDays(30);

			_context.RefreshTokens.Update(existingToken);
			await _context.SaveChangesAsync();
		}
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