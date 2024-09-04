using Events.Domain.Enums;

namespace Events.Application.Auth;

public interface IUser
{
	Guid Id { get; }
	
	string Email { get; }
	
	string Password { get; }
	
	//Role Role { get; }
}
