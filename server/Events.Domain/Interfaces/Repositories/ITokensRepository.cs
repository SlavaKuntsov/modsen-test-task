using Events.Domain.Enums;
using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface ITokensRepository
{
	public Task<RefreshTokenModel?> GetRefreshToken(string refreshToken);

	public Task UpdateRefreshToken(Guid userId, Role role, RefreshTokenModel newRefreshToken);

	public Task DeleteRefreshToken(string refreshToken);
}
