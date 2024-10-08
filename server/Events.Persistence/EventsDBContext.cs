﻿using Events.Persistence.Configurations;
using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Persistence;

public class EventsDBContext(
	DbContextOptions<EventsDBContext> options) : DbContext(options)
{
	public DbSet<EventEntity> Events { get; set; }
	public DbSet<ParticipantEntity> Participants { get; set; }
	public DbSet<EventParticipantEntity> EventsParticipants { get; set; }
	public DbSet<AdminEntity> Admins { get; set; }
	public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDBContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
