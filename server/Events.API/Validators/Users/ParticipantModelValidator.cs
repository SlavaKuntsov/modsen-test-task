using Events.Application.Handlers.Users;

using FluentValidation;

namespace Events.API.Validators.Users;

public class UpdateParticipantCommandValidator : BaseCommandValidator<UpdateParticipantCommand>
{
	public UpdateParticipantCommandValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("FirstName cannot be null or empty.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("LastName cannot be null or empty.");

		RuleFor(x => x.DateOfBirth)
			.Must(BeAValidDate).WithMessage("Date of birth must be in the format dd-MM-yyyy.")
			.NotEmpty().WithMessage("Date of birth cannot be null or empty.");
	}

	private bool BeAValidDate(DateTime dateOfBirth)
	{
		// Проверяем, что дата соответствует формату
		return dateOfBirth != default(DateTime);
	}
}

