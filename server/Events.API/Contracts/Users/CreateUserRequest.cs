using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Users;

public class CreateUserRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }

	//[Required]
	//[Compare(nameof(Password))]
	//public string ConfirmPassword { get; set; }
}
