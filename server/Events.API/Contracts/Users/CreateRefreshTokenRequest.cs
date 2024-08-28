using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Users;

public class CreateRefreshTokenRequest
{
	[Required]
	public string RefreshToken { get; set; }
}
