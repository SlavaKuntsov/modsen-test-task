using Events.Persistence.Configurations;
using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence;

public class EventsDBContext(
	DbContextOptions<EventsDBContext> options) : DbContext(options)
{
	public DbSet<EventEntity> Events { get; set; }
	public DbSet<ParticipantEntity> Participants { get; set; }
	//public DbSet<EventParticipantConfiguration> EventParticipantConfigurations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDBContext).Assembly);

		//modelBuilder.ApplyConfiguration(new EventConfiguration());
		//modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
		//modelBuilder.ApplyConfiguration(new EventParticipantConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
