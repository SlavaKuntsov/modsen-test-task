using CSharpFunctionalExtensions;

using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<IList<T>> Get<T>(CancellationToken cancellationToken) where T : IUser;
	public Task<T?> Get<T>(Guid id, CancellationToken cancellationToken) where T : IUser;
	public Task<T?> Get<T>(string email, CancellationToken cancellationToken) where T : IUser;
	public Task<ParticipantModel?> Get(string email, string password, CancellationToken cancellationToken);

	public Task<Result<Guid>> Create<T>(T user, RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken) where T : IUser;

	public Task<ParticipantModel> Update(ParticipantModel particantModel, CancellationToken cancellationToken);

	public Task Delete<T>(Guid eventId, CancellationToken cancellationToken);

	public Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive, CancellationToken cancellationToken);

	public Task<bool> IsExists(Guid eventId, CancellationToken cancellationToken);
}
