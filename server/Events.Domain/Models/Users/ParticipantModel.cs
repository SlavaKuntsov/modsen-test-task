using System.Globalization;

using CSharpFunctionalExtensions;

using Events.Domain.Enums;
using Events.Domain.Validators.Users;

namespace Events.Domain.Models.Users;

public class ParticipantModel : UserModel
{
	public const string DATE_FORMAT = "dd-MM-yyyy";
	public const string DATE_TIME_FORMAT = "dd-MM-yyyy HH:mm";

	public string FirstName { get; private set; } = string.Empty;

	public string LastName { get; private set; } = string.Empty;

	public DateTime DateOfBirth { get; private set; }

	public DateTime? EventRegistrationDate { get; private set; } = null;

	public IList<EventModel> Events { get; private set; } = [];

	public ParticipantModel() { }

	private ParticipantModel(Guid id, string email, string password, Role role, string firstName, string lastName, DateTime dateOfBirth) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		//EventRegistrationDate = eventRegistration;
	}

	public static Result<ParticipantModel> Create(Guid id, string email, string password, Role role, string firstName, string lastName, string dateOfBirth)
	{
		DateTime dateOfBirthTime;
		//DateTime eventRegistrationDateTime;

		DateTime.TryParseExact(dateOfBirth, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirthTime);
		//DateTime.TryParseExact(eventRegistrationDate, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out eventRegistrationDateTime);

		ParticipantModel model = new(id, email, password, role, firstName, lastName, dateOfBirthTime);

		var validator = new ParticipantModelValidator();
		var validationResult = validator.Validate(model);

		if (!validationResult.IsValid)
			return Result.Failure<ParticipantModel>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

		return model;
	}
}