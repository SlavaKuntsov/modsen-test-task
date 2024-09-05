using CSharpFunctionalExtensions;

using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
	public Task<ParticipantModel?> Get(Guid id);
	public Task<AdminModel?> GetAdmin(Guid id);
	public Task<ParticipantModel?> Get(string email);
	public Task<AdminModel?> GetAdmin(string email);
	public Task<ParticipantModel?> Get(string email, string password);

	public Task<Result<Guid>> Create(ParticipantModel user, RefreshTokenModel refreshTokenModel);
	public Task<Result<Guid>> Create(AdminModel user, RefreshTokenModel refreshTokenModel);

	public Task<AdminModel> ChangeAdminActivation(Guid id, bool isActive);
}
