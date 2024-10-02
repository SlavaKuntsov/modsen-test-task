using Events.Application.Handlers.Events;
using Events.Domain.Constants;

using FluentValidation;

namespace Events.API.Validators.Events;

public class CreateEventCommandValidator : BaseCommandValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be null or empty.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be null or empty.");

        RuleFor(x => x.EventDateTime)
            .Must(BeAValidDate).WithMessage($"Date of event must be in the format {DateTimeConst.DATE_TIME_FORMAT}.")
            .NotEmpty().WithMessage("Date of event cannot be null or empty.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category cannot be null or empty.");

        RuleFor(x => x.MaxParticipants)
            .NotEmpty().WithMessage("MaxParticipants cannot be null or empty.")
            .GreaterThan(0).WithMessage("MaxParticipants must be greater than 0.");
    }

    private bool BeAValidDate(DateTime dateOfBirth)
    {
        // Проверяем, что дата соответствует формату
        return dateOfBirth != default;
    }
}

