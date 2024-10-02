using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Users;

public class CreateAdminRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
	public string PasswordConfirmation { get; set; }

	[Required]
	public string Role { get; set; }
}
