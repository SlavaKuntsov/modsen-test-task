using Events.Domain.Interfaces.Services;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace Events.API.Handlers;

public class ActiveAdminHandler : AuthorizationHandler<ActiveAdminRequirement>
{
	private readonly IUsersServices _usersServices;

	public ActiveAdminHandler(IUsersServices usersServices)
	{
		_usersServices = usersServices;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveAdminRequirement requirement)
	{
		var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

		if (userIdClaim == null)
		{
			context.Fail();
			return;
		}

		Guid userId = Guid.Parse(userIdClaim.Value);

		if (context.User.IsInRole("Admin"))
		{
			var admin = await _usersServices.GetOrAuthorizeAdmin(userId);

			if (admin.IsFailure || !admin.Value.IsActiveAdmin)
			{
				context.Fail(); // Администратор неактивен
				return;
			}
		}

		context.Succeed(requirement);
	}
}
