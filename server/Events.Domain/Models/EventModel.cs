namespace Events.Domain.Models;

public class EventModel
{
	public Guid Id { get; private set; }

	public string Title { get; private set; } = string.Empty;

	public string Description { get; private set; } = string.Empty;

	public DateTime EventDateTime { get; private set; }

	public string Location { get; private set; } = string.Empty;

	public string Category { get; private set; } = string.Empty;

	public int MaxParticipants { get; private set; } = 0;

	public int ParticipantsCount { get; private set; } = 0;

	public byte[] Image { get; private set; } = [];

	public void SetImage(byte[] image) => Image = image;

	public EventModel() { }

	public EventModel(Guid id, string title, string description, DateTime eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl)
	{
		Id = id;
		Title = title;
		Description = description;
		EventDateTime = eventDateTime;
		Location = location;
		Category = category;
		MaxParticipants = maxParticipants;
		Image = imageUrl;
	}

	public EventModel(Guid id, string title, string description, DateTime eventDateTime, string location, string category, int maxParticipants, int participantsCount, byte[] imageUrl)
	{
		Id = id;
		Title = title;
		Description = description;
		EventDateTime = eventDateTime;
		Location = location;
		Category = category;
		MaxParticipants = maxParticipants;
		ParticipantsCount = participantsCount;
		Image = imageUrl;
	}
}
