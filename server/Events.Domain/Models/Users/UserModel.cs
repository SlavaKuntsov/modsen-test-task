using Events.Domain.Enums;
using Events.Domain.Interfaces;

namespace Events.Domain.Models.Users;

public abstract class UserModel : IUser
{
	public Guid Id { get; protected set; }

	public string Email { get; protected set; } = string.Empty;

	public string Password { get; protected set; } = string.Empty;

	public Role Role { get; protected set; }

	public UserModel() { }

	public UserModel(Guid id, string email, string password, Role role)
	{
		Id = id;
		Email = email;
		Password = password;
		Role = role;
	}
}
