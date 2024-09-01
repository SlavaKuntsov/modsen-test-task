using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Users;

public class CreateParticipantRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	[Compare(nameof(Password))]
	public string PasswordConfirmation { get; set; }

	[Required]
	public string Role { get; set; }
	//public Role Role { get; set; } = Role.User;

	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	[Required]
	//[DateFormat("dd-MM-yyyy")]
	public string DateOfBirth { get; set; }
}
