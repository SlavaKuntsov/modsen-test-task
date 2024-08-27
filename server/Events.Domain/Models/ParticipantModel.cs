using CSharpFunctionalExtensions;

namespace Events.Domain.Models;

public class ParticipantModel
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public DateTime DateOfBirth { get; set; }

	public DateTime? EventRegistrationDate { get; set; } = null;

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public IList<EventModel> Events { get; set; } = [];

	public ParticipantModel() { }

	private ParticipantModel(Guid id, string email, string password)
	{
		Id = id;
		Email = email;
		Password = password;
	}

	public static Result<ParticipantModel> Create(Guid id, string email, string password)
	{
		if (string.IsNullOrEmpty(email))
			return Result.Failure<ParticipantModel>("Email cannot be null or empty.");

		return Result.Success(new ParticipantModel(id, email, password));
	}
}
