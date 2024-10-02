using Events.API.Middlewares;
using Events.Application.Handlers.Users;
using Events.Domain.Models.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace Events.API.Middlewares;

public class ActiveAdminHandler : AuthorizationHandler<ActiveAdminRequirement>
{
	private readonly IMediator _mediator;

	public ActiveAdminHandler(IMediator mediator)
	{
		_mediator = mediator;
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
			var admin = await _mediator.Send(new GetOrAuthorizeUserQuery<AdminModel>(userId));

			if (admin == null || !admin.IsActiveAdmin)
			{
				context.Fail(); // Администратор не найден или неактивен
				return;
			}

		}

		context.Succeed(requirement);
	}
}
