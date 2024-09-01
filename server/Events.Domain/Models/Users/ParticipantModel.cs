using System.ComponentModel.DataAnnotations;
using System.Globalization;

using CSharpFunctionalExtensions;

using Events.Domain.Enums;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Events.Domain.Models.Users;

public class ParticipantModel : UserModel
{
	//public Guid Id { get; set; }

	//public string Email { get; set; } = string.Empty;

	//public string Password { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public DateTime DateOfBirth { get; set; }

	public DateTime? EventRegistrationDate { get; set; } = null;

	public IList<EventModel> Events { get; set; } = [];

	public ParticipantModel()
	{

	}

	private ParticipantModel(Guid id, string email, string password, Role role, string firstName, string lastName, DateTime dateOfBirth) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
	}

	public static Result<ParticipantModel> Create(Guid id, string email, string password, Role role, string firstName, string lastName, string dateOfBirth)
	{
		if (string.IsNullOrEmpty(email))
			return Result.Failure<ParticipantModel>("Email cannot be null or empty.");

		DateTime dateTime;

		string format = "dd-MM-yyyy";

		if (!DateTime.TryParseExact(dateOfBirth, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
		{
			return Result.Failure<ParticipantModel>($"Date of birth must be in the format {format}");
		}

		return Result.Success(new ParticipantModel(id, email, password, role, firstName, lastName, dateTime));
	}
}