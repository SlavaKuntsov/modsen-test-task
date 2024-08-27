using Events.Application.Services;
using Events.Domain.Interfaces.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Events.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IEventsServices, EventsService>();
		services.AddScoped<IUsersServices, UsersService>();

		return services;
	}
}
