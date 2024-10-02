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

	public async Task<RefreshTokenModel?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken)
	{
		var entity = await _context
			.RefreshTokens
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.Token == refreshToken, cancellationToken);

		if (entity == null)
			return null;

		return _mapper.Map<RefreshTokenModel>(entity);
	}

	// TODO - его логика добавлена в Create метод UsersRepository
	public async Task SaveRefreshToken(RefreshTokenModel refreshToken, CancellationToken cancellationToken)
	{
		var entity = _mapper.Map<RefreshTokenEntity>(refreshToken);

		await _context.RefreshTokens.AddAsync(entity, cancellationToken);

		await _context.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateRefreshToken(Guid userId, Role role, RefreshTokenModel newRefreshToken, CancellationToken cancellationToken)
	{
		RefreshTokenEntity? existingToken = null;

		if (role == Role.Admin)
		{
			existingToken = await _context.RefreshTokens
				.FirstOrDefaultAsync(rt => rt.AdminId == userId, cancellationToken);
		}
		else if (role == Role.User)
		{
			existingToken = await _context.RefreshTokens
				.FirstOrDefaultAsync(rt => rt.UserId == userId, cancellationToken);
		}

		if (existingToken != null)
		{
			existingToken.Token = newRefreshToken.Token;
			existingToken.ExpiresAt = newRefreshToken.ExpiresAt;
			existingToken.CreatedAt = newRefreshToken.CreatedAt;

			_context.RefreshTokens.Update(existingToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
		else
			await SaveRefreshToken(newRefreshToken, cancellationToken);
	}

	public async Task DeleteRefreshToken(string refreshToken, CancellationToken cancellationToken)
	{
		var token = await _context
			.RefreshTokens
			.FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

		if (token != null)
		{
			_context.RefreshTokens.Remove(token);
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}