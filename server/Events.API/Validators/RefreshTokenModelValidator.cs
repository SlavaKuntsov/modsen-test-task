using Events.Domain.Models;

using FluentValidation;

namespace Events.API.Validators.Users;

public class RefreshTokenModelValidator : AbstractValidator<RefreshTokenModel>
{
	public RefreshTokenModelValidator()
	{
		RuleFor(user => user.Id)
			.NotNull();

		RuleFor(x => x.Token)
			.NotEmpty().WithMessage("Title cannot be null or empty.");

		RuleFor(x => x.ExpiresAt)
			.NotEmpty().WithMessage("Date of token expire cannot be null or empty.");

		RuleFor(x => x.CreatedAt)
			.NotEmpty().WithMessage("Date of token create cannot be null or empty.");

		RuleFor(x => x.IsRevoked)
			.NotNull().WithMessage("IsRevoked cannot be null or empty.");
	}
}

