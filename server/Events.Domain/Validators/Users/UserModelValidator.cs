using Events.Domain.Models.Users;

using FluentValidation;

namespace Events.Domain.Validators.Users;

public class UserModelValidator : AbstractValidator<UserModel>
{
	public UserModelValidator()
	{
		RuleFor(user => user.Id)
			.NotNull();

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email cannot be null or empty.")
			.EmailAddress().WithMessage("Invalid email format.");

		RuleFor(m => m.Password)
			.NotEmpty().WithMessage("Paword cannot be null or empty.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]").WithMessage("Password must contain at least one number.")
			.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

		RuleFor(user => user.Role)
			.NotNull()
			.IsInEnum();
	}
}
