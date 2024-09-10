using Events.Application.Auth;
using Events.Domain.Enums;

namespace Events.Persistence.Entities;

public class AdminEntity : IUser
{
	public Guid Id { get; set; }
	
	public string Email { get; set; } = string.Empty;
	
	public string Password { get; set; } = string.Empty;

	public Role Role { get; set; } 

	public bool IsActiveAdmin { get; set; } = false; 

	public RefreshTokenEntity RefreshToken { get; set; } 
}