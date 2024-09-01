using System.Globalization;

using CSharpFunctionalExtensions;

using Events.Domain.Models.Users;

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

	// при изменении IList<ParticipantModel> Participants менять количество зарегистрированных участников
	public int ParticipantsCount { get; set; } = 0;

	public string ImageUrl { get; set; } = string.Empty;

	public IList<ParticipantModel> Participants { get; set; } = [];

	public EventModel() { }

	private EventModel(Guid id, string title, string description, DateTime eventDateTime, string location, string category, int maxParticipants, string imageUrl)
	{
		Id = id;
		Title = title;
		Description = description;
		EventDateTime = eventDateTime;
		Location = location;
		Category = category;
		MaxParticipants = maxParticipants;
		ImageUrl = imageUrl;
	}

	public static Result<EventModel> Create(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, string imageUrl)
	{
		if (string.IsNullOrEmpty(title))
			return Result.Failure<EventModel>("Title cannot be null or empty.");

		DateTime dateTime;

		string format = "dd-MM-yyyy";

		if (!DateTime.TryParseExact(eventDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
		{
			return Result.Failure<EventModel>($"Date of event must be in the format {format}");
		}

		return Result.Success(new EventModel(id, title, description, dateTime, location, category, maxParticipants, imageUrl));
	}
}
