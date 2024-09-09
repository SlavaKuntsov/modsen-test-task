namespace Events.Persistence.Entities;

public class EventEntity
{
	public Guid Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public DateTime EventDateTime { get; set; }

	public string Location { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public int MaxParticipants { get; set; }

	public int ParticipantsCount { get; set; }

	//public string ImageUrl { get; set; } = string.Empty;
	public byte[] Image { get; set; } = [];

	public virtual IList<EventParticipantEntity> EventParticipants { get; set; } = [];
}
