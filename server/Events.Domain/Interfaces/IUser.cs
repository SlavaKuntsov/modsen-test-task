using Events.Domain.Enums;

namespace Events.Domain.Interfaces;

public interface IUser
{
	Guid Id { get; }

	string Email { get; }

	string Password { get; }

	Role Role { get; }
}
