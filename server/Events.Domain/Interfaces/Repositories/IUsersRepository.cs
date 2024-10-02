using CSharpFunctionalExtensions;

using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<IList<T>> Get<T>() where T : IUser;
	public Task<T?> Get<T>(Guid id) where T : IUser;
	public Task<T?> Get<T>(string email) where T : IUser;
	public Task<ParticipantModel?> Get(string email, string password);

	public Task<Result<Guid>> Create<T>(T user, RefreshTokenModel refreshTokenModel) where T : IUser;

	public Task<ParticipantModel> Update(ParticipantModel particantModel);

	public Task Delete<T>(Guid eventId);

	public Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive);

	public Task<bool> IsExists(Guid eventId);
}
