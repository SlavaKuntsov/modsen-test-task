using System.Collections.Generic;
using System.Reflection.Emit;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence;

public class EventsDBContext(
	DbContextOptions<EventsDBContext> options) : DbContext(options)
{
	public DbSet<CourseEntity> Courses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Альтернативная запись для всех DBSet
		//modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningDbContext).Assembly);

		modelBuilder.ApplyConfiguration(new CourseConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
