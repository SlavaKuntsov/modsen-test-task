using Events.Domain.Enums;

namespace Events.Application.Common.Auth;

public interface IJwt
{
    public string GenerateAccessToken(Guid id, Role role);

    public string GenerateRefreshToken();

    public Task<Guid> ValidateRefreshToken(string refreshToken);

    public int GetRefreshTokenExpirationDays();
}