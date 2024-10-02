using Events.Domain.Enums;

namespace Events.Domain.Models.Users;

public class AdminModel : UserModel
{
	public bool IsActiveAdmin { get; private set; } = false;

	public AdminModel() { }

	public AdminModel(Guid id, string email, string password, Role role) : base(id, email, password, role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
	}
}
