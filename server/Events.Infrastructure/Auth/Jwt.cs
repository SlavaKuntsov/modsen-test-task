using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Events.Application.Auth;
using Events.Domain.Models;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Events.Infrastructure.Auth;

public class Jwt : IJwt
{
	private readonly JwtModel _options;

	public Jwt(IOptions<JwtModel> options)
	{
		_options = options.Value;
	}

	public string Generate(ParticipantModel user)
	{
		Claim[] claims =
		[
			new ("Id", user.Id.ToString()),
			new ("Admin", "true")
		];

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
			signingCredentials: signingCredentials);

		var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

		return tokenValue;
	}
}
