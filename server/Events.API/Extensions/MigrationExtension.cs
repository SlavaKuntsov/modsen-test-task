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

		try
		{
			eventsDbContext.Database.Migrate();
		}
		catch (Exception ex)
		{
			// Логируйте исключение или выводите его в консоль
			Console.WriteLine($"Migration error: {ex.Message}");
			throw; // или обработайте по своему усмотрению
		}
	}

}
