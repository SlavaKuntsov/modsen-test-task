namespace Events.API.Contracts.Users;

public class GetAdminResponse
{
	public Guid Id { get; set; }

	public string Email { get; set; }

	public bool IsActiveAdmin { get; set; }
}
