using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistence.Configurations;

public partial class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipantEntity>
{
	public void Configure(EntityTypeBuilder<EventParticipantEntity> builder)
	{
		builder.HasKey(e => new { e.EventId, e.ParticipantId });

		builder.ToTable("EventParticipant");
	}
}
