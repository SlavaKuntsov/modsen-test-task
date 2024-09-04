using Events.Domain.Enums;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Persistence.Entities;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence.Repositories;

public class TokensRepository : ITokensRepository
{
	private readonly EventsDBContext _context;
	private readonly IMapper _mapper;

	public TokensRepository(EventsDBContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
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

	// TODO - его логика добавлена в Create метод UsersRepository
	public async Task SaveRefreshToken(RefreshTokenModel refreshToken)
	{
		var entity = _mapper.Map<RefreshTokenEntity>(refreshToken);

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