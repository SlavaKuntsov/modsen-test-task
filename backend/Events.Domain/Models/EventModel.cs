using CSharpFunctionalExtensions;

namespace Events.Domain.Models;

public class EventModel
{
	// private set
	public Guid Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public DateTime EventDateTime { get; set; }

	public string Location { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public int MaxParticipants { get; set; }

	public string ImageUrl { get; set; } = string.Empty;

	public ICollection<ParticipantModel> Participants { get; set; } = [];

	public EventModel() { }

	private EventModel(Guid id, string title)
	{
		Id = id;
		Title = title;
	}

	public static Result<EventModel> Create(Guid id, string title)
	{
		if (string.IsNullOrEmpty(title))
			return Result.Failure<EventModel>("Title cannot be null or empty.");

		return Result.Success(new EventModel(id, title));
	}
}
