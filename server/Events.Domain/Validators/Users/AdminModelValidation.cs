using Events.Domain.Models.Users;

using FluentValidation;

namespace Events.Domain.Validators.Users;

public class AdminModelValidation : AbstractValidator<AdminModel>
{
	public AdminModelValidation()
	{
		Include(new UserModelValidator());

		RuleFor(x => x.IsActiveAdmin)
				.NotEmpty().WithMessage("IsActiveAdmin cannot be null or empty.");
	}
}
