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

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password cannot be null or empty.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

		RuleFor(user => user.Role)
			.NotNull()
			.IsInEnum();
	}
}
