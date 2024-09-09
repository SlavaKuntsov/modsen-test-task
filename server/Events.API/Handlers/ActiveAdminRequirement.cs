using Microsoft.AspNetCore.Authorization;

namespace Events.API.Handlers;

public class ActiveAdminRequirement : IAuthorizationRequirement
{
	public ActiveAdminRequirement() { }
}
