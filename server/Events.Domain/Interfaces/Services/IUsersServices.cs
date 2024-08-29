using CSharpFunctionalExtensions;

using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IUsersServices
{
	public Task<Result<AuthResultModel>> Login(string email, string password);

	public Task<Result<AuthResultModel>> Registration(string email, string password);

	public Task<Result<AuthResultModel>> RefreshToken(string refreshToken);

	public Task<Result<ParticipantModel>> Authorize(Guid id);
}
