using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence;

public class EventsDBContext(
	DbContextOptions<EventsDBContext> options) : DbContext(options)
{
	public DbSet<EventEntity> Events { get; set; }
	public DbSet<ParticipantEntity> Participants { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDBContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
