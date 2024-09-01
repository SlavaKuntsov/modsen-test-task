using CSharpFunctionalExtensions;
using Events.Domain.Models.Users;

namespace Events.Application.Auth;

public interface IJwt
{
	public string GenerateAccessToken(Guid id);

	public string GenerateRefreshToken();

	public Task<Guid> ValidateRefreshToken(string refreshToken);

	public int GetRefreshTokenExpirationDays();
}