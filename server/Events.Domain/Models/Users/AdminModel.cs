using CSharpFunctionalExtensions;

using Events.Domain.Enums;

namespace Events.Domain.Models.Users;

public class AdminModel : UserModel
{
	public bool IsActiveAdmin { get; set; } = false;

	public AdminModel()
	{

	}

	private AdminModel(Guid id, string email, string password, Role role) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
	}

	public static Result<AdminModel> Create(Guid id, string email, string password, Role role)
	{
		if (string.IsNullOrEmpty(email))
			return Result.Failure<AdminModel>("Email cannot be null or empty.");

		return Result.Success(new AdminModel(id, email, password, role));
	}
}
