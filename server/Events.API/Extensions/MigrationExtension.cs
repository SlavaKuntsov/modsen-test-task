using Events.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Events.API.Extensions;

public static class MigrationExtension
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using IServiceScope scope = app.ApplicationServices.CreateAsyncScope();

		using EventsDBContext eventsDbContext =
				scope.ServiceProvider.GetRequiredService<EventsDBContext>();

		eventsDbContext.Database.Migrate();
	}
}
