using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<ParticipantModel?> Get(Guid id);
	public Task<ParticipantModel?> Get(string email);
	public Task<ParticipantModel?> Get(string email, string password);

	public Task<Guid> Create(ParticipantModel user);

	public Task<RefreshTokenModel?> GetRefreshToken(string refreshToken);

	public Task SaveRefreshToken(RefreshTokenModel refreshToken);

	public Task UpdateRefreshToken(Guid userId, RefreshTokenModel newRefreshToken);

	public Task DeleteRefreshToken(string refreshToken);
}
