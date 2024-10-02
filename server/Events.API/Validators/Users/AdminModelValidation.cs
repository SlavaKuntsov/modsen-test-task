using Events.Domain.Models.Users;

using FluentValidation;

namespace Events.API.Validators.Users;

public class AdminModelValidation : AbstractValidator<AdminModel>
{
	public AdminModelValidation()
	{
		Include(new UserModelValidator());

		RuleFor(x => x.IsActiveAdmin)
			.NotNull().WithMessage("IsActiveAdmin cannot be null or empty.");
	}
}
