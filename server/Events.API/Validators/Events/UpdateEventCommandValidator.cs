using Events.Application.Handlers.Events;
using FluentValidation;

namespace Events.API.Validators.Events;

public class UpdateEventCommandValidator : BaseCommandValidator<UpdateEventCommand>
{
	public UpdateEventCommandValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title cannot be null or empty.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description cannot be null or empty.");

		RuleFor(x => x.EventDateTime)
			.Must(BeAValidDate).WithMessage("Date of event must be in the format dd-MM-yyyy.")
			.NotEmpty().WithMessage("Date of event cannot be null or empty.");

		RuleFor(x => x.Category)
			.NotEmpty().WithMessage("Category cannot be null or empty.");

		RuleFor(x => x.MaxParticipants)
			.NotEmpty().WithMessage("MaxParticipants cannot be null or empty.")
			.GreaterThan(0).WithMessage("MaxParticipants must be greater than 0.");

		RuleFor(x => x.ParticipantsCount)
			.NotNull().WithMessage("ParticipantsCount cannot be null or empty.");
	}

	private bool BeAValidDate(DateTime dateOfBirth)
	{
		// Проверяем, что дата соответствует формату
		return dateOfBirth != default;
	}
}
