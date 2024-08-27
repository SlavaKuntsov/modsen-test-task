using Events.Domain.Interfaces;
using Events.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Persistence;

public static class PersistenceExtensions
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<EventsDBContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString(nameof(EventsDBContext)));
		});

		services.AddScoped<IEventsRepository, EventsRepository>();
		services.AddScoped<IUsersRepository, UsersRepository>();

		return services;
	}
}