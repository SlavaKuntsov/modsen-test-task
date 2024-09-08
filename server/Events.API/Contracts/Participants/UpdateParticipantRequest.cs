using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Participants;

public class UpdateParticipantRequest
{
	[Required]
	public Guid Id { get; set; }

	//[Required]
	//[EmailAddress]
	//public string Email { get; set; }

	//[Required]
	//public string Password { get; set; }

	//[Required]
	//[Compare(nameof(Password))]
	//public string PasswordConfirmation { get; set; }

	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	[Required]
	public string DateOfBirth { get; set; }

	//[Required]
	//public string EventRegistrationDate { get; set; }
}
