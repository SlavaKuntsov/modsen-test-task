using Events.Domain.Enums;

namespace Events.Application.DTOs;

public class UserDto
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public Role Role { get; set; }
}
