using CSharpFunctionalExtensions;

using Events.Domain.Enums;
using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Services;

public interface IUsersServices
{
	public Task<Result<AuthResultModel>> Login(string email, string password);

	public Task<Result<AuthResultModel>> ParticipantRegistration(string email, string password, Role role, string firstName, string lastName, string dateOfBirth);

	public Task<Result<AuthResultModel>> AdminRegistration(string email, string password, Role role);

	public Task<Result<AuthResultModel>> RefreshToken(string refreshToken);

	public Task<Result<ParticipantModel>> Authorize(Guid id);
}
