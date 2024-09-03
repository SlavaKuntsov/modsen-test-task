using CSharpFunctionalExtensions;

using Events.Domain.Enums;
using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<ParticipantModel?> Get(Guid id);
	public Task<ParticipantModel?> Get(string email);
	public Task<ParticipantModel?> Get(string email, string password);

	public Task<Result<Guid>> Create(ParticipantModel user, RefreshTokenModel refreshTokenModel);
	public Task<Result<Guid>> Create(AdminModel user, RefreshTokenModel refreshTokenModel);

	public Task<RefreshTokenModel?> GetRefreshToken(string refreshToken);

	//public Task SaveRefreshToken(RefreshTokenModel refreshToken);

	public Task UpdateRefreshToken(Guid userId, Role role, RefreshTokenModel newRefreshToken);

	public Task DeleteRefreshToken(string refreshToken);
}
