using Events.Domain.Enums;

namespace Events.Application.DTOs;

public  class AdminDto
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public string Role { get; set; } = string.Empty;

	public bool IsActiveAdmin { get; set; }
}
