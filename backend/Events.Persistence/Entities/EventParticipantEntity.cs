namespace Events.Persistence.Entities;

public class EventParticipantEntity
{
	public Guid EventId { get; set; }

	public Guid ParticipantId { get; set; }
}