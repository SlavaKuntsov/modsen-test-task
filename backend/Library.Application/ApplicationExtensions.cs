using Microsoft.Extensions.DependencyInjection;

namespace Library.Application;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<CoursesService>();

		return services;
	}
}
