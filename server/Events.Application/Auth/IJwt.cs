using Events.Domain.Models;

namespace Events.Application.Auth;

public interface IJwt
{
	public string Generate(ParticipantModel user);

	public string GenerateAccessToken(ParticipantModel participant);

	public string GenerateRefreshToken();

	public Task<Guid> ValidateRefreshToken(string refreshToken);

	public int GetRefreshTokenExpirationDays();
}