using Events.Domain.Enums;
using Events.Domain.Interfaces;

namespace Events.Persistence.Entities;

public class ParticipantEntity : IUser
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public Role Role { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public DateTime DateOfBirth { get; set; }

	public IList<EventParticipantEntity> Events { get; set; } = [];

	public RefreshTokenEntity RefreshToken { get; set; } = null!;
}
