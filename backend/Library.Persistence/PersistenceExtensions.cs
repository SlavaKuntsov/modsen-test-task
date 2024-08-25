using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Persistence;

public static class PersistenceExtensions
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<LibraryDBContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString(nameof(LibraryDBContext)));
		});

		services.AddScoped<ICourseRepository, CourseRepository>();

		return services;
	}
}