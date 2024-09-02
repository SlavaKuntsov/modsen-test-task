using CSharpFunctionalExtensions;

using Events.Domain.Enums;
using Events.Domain.Validators.Users;

namespace Events.Domain.Models.Users;

public class AdminModel : UserModel
{
	public bool IsActiveAdmin { get; private set; } = false;

	public AdminModel() { }

	private AdminModel(Guid id, string email, string password, Role role) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
	}

	public static Result<AdminModel> Create(Guid id, string email, string password, Role role)
	{
		AdminModel model = new AdminModel(id, email, password, role);

		var validator = new AdminModelValidation();
		var validationResult = validator.Validate(model);

		if (!validationResult.IsValid)
			return Result.Failure<AdminModel>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

		return model;
	}
}
