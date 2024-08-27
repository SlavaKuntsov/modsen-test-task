namespace Events.Persistence.Entities;

public class ParticipantEntity
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public DateTime DateOfBirth { get; set; }

	public DateTime? EventRegistrationDate { get; set; } = null;

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public IList<EventEntity> Events { get; set; } = [];
}
