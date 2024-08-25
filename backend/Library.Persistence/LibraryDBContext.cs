using Microsoft.EntityFrameworkCore;

namespace Library.Persistence;

public class LibraryDBContext(
	DbContextOptions<LibraryDBContext> options) : DbContext(options)
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
