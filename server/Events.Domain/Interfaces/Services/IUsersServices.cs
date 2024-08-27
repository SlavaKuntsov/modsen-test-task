using CSharpFunctionalExtensions;

using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IUsersServices
{
	public Task<Result<string>> Login(string email, string password);

	public Task<Result<Guid>> Register(string email, string passwordHash);
}
