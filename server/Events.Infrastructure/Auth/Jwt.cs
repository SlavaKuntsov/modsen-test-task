﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Events.Application.Auth;
using Events.Domain.Enums;
using Events.Domain.Interfaces.Repositories;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Events.Infrastructure.Auth;

public class Jwt : IJwt
{
	private readonly JwtModel _jwtOptions;
	private readonly ITokensRepository _tokensRepository;

	public Jwt(IOptions<JwtModel> jwtOptions, ITokensRepository tokensRepository)
	{
		_jwtOptions = jwtOptions.Value;
		_tokensRepository = tokensRepository;
	}

	public string GenerateAccessToken(Guid id, Role role)
	{
		var claims = new[]
		{
			new Claim("Id", id.ToString()),
			new Claim(ClaimTypes.Role, role.ToString())
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
		var storedToken = await _tokensRepository.GetRefreshToken(refreshToken);

		if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
			return Guid.Empty;

		if (storedToken.AdminId.HasValue && storedToken.AdminId.Value != Guid.Empty)
		{
			return storedToken.AdminId.Value;
		}
		else if (storedToken.UserId.HasValue && storedToken.UserId.Value != Guid.Empty)
		{
			return storedToken.UserId.Value;
		}

		return Guid.Empty;
	}

	public int GetRefreshTokenExpirationDays()
	{
		return _jwtOptions.RefreshTokenExpirationDays;
	}
}
