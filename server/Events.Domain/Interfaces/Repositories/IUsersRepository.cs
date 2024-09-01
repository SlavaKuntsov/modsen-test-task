using Events.Domain.Enums;
using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<ParticipantModel?> Get(Guid id);
	public Task<ParticipantModel?> Get(string email);
	public Task<ParticipantModel?> Get(string email, string password);

	public Task<Guid> Create(ParticipantModel user);
	public Task<Guid> Create(AdminModel user);

	public Task<RefreshTokenModel?> GetRefreshToken(string refreshToken);

	public Task SaveRefreshToken(RefreshTokenModel refreshToken);

	public Task UpdateRefreshToken(Guid userId, Role role, RefreshTokenModel newRefreshToken);

	public Task DeleteRefreshToken(string refreshToken);
}
