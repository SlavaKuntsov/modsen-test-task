using CSharpFunctionalExtensions;

using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IUsersServices
{
	public Task<Result<ParticipantModel>> Get(string email, string password);
	public Task<Result<ParticipantModel>> Get(string email);

	public Task<Result<Guid>> Create(ParticipantModel user);
}
