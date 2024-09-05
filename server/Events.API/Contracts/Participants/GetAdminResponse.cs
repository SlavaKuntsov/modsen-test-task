namespace Events.API.Contracts.Participants;

public class GetAdminResponse
{
	public Guid Id { get; set; }

	public string Email { get; set; }

	public string Role { get; set; }
}
