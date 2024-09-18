using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Handlers;

public class ActiveAdminRequirement : IAuthorizationRequirement
{
	public ActiveAdminRequirement() { }
}
