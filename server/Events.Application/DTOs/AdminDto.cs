using Events.Domain.Enums;

namespace Events.Application.DTOs;

public  class AdminDto
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public Role Role { get; set; }

	public bool IsActiveAdmin { get; set; }
}
