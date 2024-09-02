using Events.Domain.Models;

using FluentValidation;

namespace Events.Domain.Validators.Users;

public class RefreshTokenModelValidator : AbstractValidator<RefreshTokenModel>
{
	public RefreshTokenModelValidator()
	{
		RuleFor(user => user.Id)
			.NotNull();

		RuleFor(x => x.Token)
				.NotEmpty().WithMessage("Title cannot be null or empty.");

		RuleFor(x => x.ExpiresAt)
			.Must(BeAValidDate).WithMessage("Date of token expire must be in the format dd-MM-yyyy.")
			.Must(BeAPastDate).WithMessage("Date of token expire must be in the past.")
			.NotEmpty().WithMessage("Date of token expire cannot be null or empty.");

		RuleFor(x => x.CreatedAt)
			.Must(BeAValidDate).WithMessage("Date of token create must be in the format dd-MM-yyyy.")
			.NotEmpty().WithMessage("Date of token create cannot be null or empty.");

		RuleFor(x => x.IsRevoked)
				.NotEmpty().WithMessage("IsRevoked cannot be null or empty.");
	}

	private bool BeAValidDate(DateTime dateOfBirth)
	{
		return dateOfBirth != default(DateTime);
	}

	private bool BeAPastDate(DateTime date)
	{
		return date < DateTime.Today;
	}
}

