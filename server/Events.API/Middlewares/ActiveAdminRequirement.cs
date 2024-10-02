using Microsoft.AspNetCore.Authorization;

namespace Events.API.Middlewares;

public class ActiveAdminRequirement : IAuthorizationRequirement
{
    public ActiveAdminRequirement() { }
}
