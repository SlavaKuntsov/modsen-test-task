using Events.Domain.Constants;
using Events.Domain.Enums;

namespace Events.Domain.Models.Users;

public class ParticipantModel : UserModel
{
	public const string DATE_TIME_FORMAT = "dd-MM-yyyy HH:mm";

	public string FirstName { get; private set; } = string.Empty;

	public string LastName { get; private set; } = string.Empty;

	public DateTime DateOfBirth { get; private set; }

	public DateTime? EventRegistrationDate { get; private set; } = null;

	public IList<EventModel> Events { get; private set; } = [];

	public ParticipantModel() { }

	public ParticipantModel(Guid id, string email, string password, Role role, string firstName, string lastName, DateTime dateOfBirth) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
	}
}