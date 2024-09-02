using Events.Domain.Models;

using FluentValidation;

namespace Events.Domain.Validators.Users;

public class EventModelValidator : AbstractValidator<EventModel>
{
	public EventModelValidator()
	{
		RuleFor(user => user.Id)
			.NotNull();

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
				.NotEmpty().WithMessage("ParticipantsCount cannot be null or empty.")
				.GreaterThan(0).WithMessage("ParticipantsCount must be greater than 0.");

		RuleFor(x => x.ImageUrl)
				.NotEmpty().WithMessage("ImageUrl cannot be null or empty.");
	}

	private bool BeAValidDate(DateTime dateOfBirth)
	{
		// Проверяем, что дата соответствует формату
		return dateOfBirth != default(DateTime);
	}
}

