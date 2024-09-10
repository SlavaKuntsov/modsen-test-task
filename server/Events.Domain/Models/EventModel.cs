using System.Globalization;

using CSharpFunctionalExtensions;

using Events.Domain.Models.Users;
using Events.Domain.Validators.Users;

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

	// при изменении IList<ParticipantModel> Participants менять количество зарегистрированных участников
	public int ParticipantsCount { get; private set; } = 0;

	//public string ImageUrl { get; private set; } = string.Empty;
	public byte[] Image { get; private set; } = [];

	//public IList<ParticipantModel> Participants { get; set; } = [];

	public EventModel() { }

	private EventModel(Guid id, string title, string description, DateTime eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl)
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
	private EventModel(Guid id, string title, string description, DateTime eventDateTime, string location, string category, int maxParticipants, int participantsCount, byte[] imageUrl)
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

	public static Result<EventModel> Create(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl)
	{
		DateTime dateTime;

		DateTime.TryParseExact(eventDateTime, ParticipantModel.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

		EventModel model = new(id, title, description, dateTime, location, category, maxParticipants, imageUrl);

		var validator = new EventModelValidator();
		var validationResult = validator.Validate(model);

		if (!validationResult.IsValid)
			return Result.Failure<EventModel>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

		return model;
	}

	public static Result<EventModel> Create(EventModel model, byte[] imageUrl)
	{
		if (model == null)
			return Result.Failure<EventModel>("Model cannot be null");

		EventModel newModel = new(model.Id, model.Title, model.Description, model.EventDateTime, model.Location, model.Category, model.MaxParticipants, model.ParticipantsCount, imageUrl);

		var validator = new EventModelValidator();
		var validationResult = validator.Validate(newModel);

		if (!validationResult.IsValid)
			return Result.Failure<EventModel>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

		return newModel;
	}
}
