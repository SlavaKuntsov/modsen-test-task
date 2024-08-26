using Microsoft.Extensions.DependencyInjection;

namespace Events.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<CoursesService>();

		return services;
	}
}
