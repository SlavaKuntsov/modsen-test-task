using Events.Domain.Models.Users;

using FluentValidation;

namespace Events.Domain.Validators.Users;

public class ParticipantModelValidator : AbstractValidator<ParticipantModel>
{
	public ParticipantModelValidator()
	{
		Include(new UserModelValidator());

		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("FirstName cannot be null or empty.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("LastName cannot be null or empty.");

		RuleFor(user => user.Role)
			.NotNull()
			.IsInEnum();

		RuleFor(x => x.DateOfBirth)
			.Must(BeAValidDate).WithMessage("Date of birth must be in the format dd-MM-yyyy.")
			.NotEmpty().WithMessage("Date of birth cannot be null or empty.");

		//RuleFor(x => x.EventRegistrationDate)
		//	.Must(BeAValidDate).WithMessage("Date of event registration must be in the format dd-MM-yyyy.")
		//	.NotEmpty().WithMessage("Date of event registration cannot be null or empty.");
	}

	private bool BeAValidDate(DateTime dateOfBirth)
	{
		// Проверяем, что дата соответствует формату
		return dateOfBirth != default(DateTime);
	}
}

