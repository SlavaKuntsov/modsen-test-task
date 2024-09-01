using Events.Application.Auth;
using Events.Domain.Enums;

namespace Events.Persistence.Entities;

public class AdminEntity : IUser
{
	public Guid Id { get; set; } // уникальный идентификатор
	
	public string Email { get; set; } = string.Empty; // почта
	
	public string Password { get; set; } = string.Empty; // пароль
	
	public Role Role { get; set; } // роль
	
	public bool IsActiveAdmin { get; set; } = false; // активный администратор

	public RefreshTokenEntity RefreshToken { get; set; } 
}