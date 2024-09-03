namespace Events.Persistence.Entities;

public class EventParticipantEntity
{
	public Guid EventId { get; set; }

	public Guid ParticipantId { get; set; }

	public DateTime EventRegistrationDate { get; set; }

	public virtual ParticipantEntity Participant { get; set; } = null!;
	public virtual EventEntity Event { get; set; } = null!;
}
