using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Events.Application.Auth;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Events.Infrastructure.Auth;

public class Jwt : IJwt
{
	private readonly JwtModel _jwtOptions;
	private readonly IUsersRepository _usersRepository;

	public Jwt(IOptions<JwtModel> jwtOptions, IUsersRepository usersRepository)
	{
		_jwtOptions = jwtOptions.Value;
		_usersRepository = usersRepository;
	}

	public string Generate(ParticipantModel user)
	{
		Claim[] claims =
		[
			new ("Id", user.Id.ToString()),
			new ("Admin", "true")
		];

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			claims: claims,
			expires:DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours),
			signingCredentials: signingCredentials);

		var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

		return tokenValue;
	}

	public string GenerateAccessToken(ParticipantModel participant)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, participant.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, participant.Email),
            // Другие необходимые клеймы
        };

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var randomBytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);

		return Convert.ToBase64String(randomBytes);
	}

	public async Task<Guid> ValidateRefreshToken(string refreshToken)
	{
		// Логика проверки токена (например, поиск в БД и валидация)
		var token = await _usersRepository.GetRefreshToken(refreshToken);

		if(token == null)
			return Guid.Empty;

		return token.UserId;
	}

	public int GetRefreshTokenExpirationDays()
	{
		return _jwtOptions.RefreshTokenExpirationDays;
	}
}
