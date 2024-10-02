using FluentValidation;

namespace Events.API.Validators;

public abstract class BaseCommandValidator<T> : AbstractValidator<T>
{
	protected BaseCommandValidator()
	{
	}
}
