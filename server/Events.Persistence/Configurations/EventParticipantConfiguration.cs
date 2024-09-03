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

		builder.Property(p => p.EventRegistrationDate)
			.IsRequired();
			//.HasColumnType("date")
			//.HasConversion(
			//	v => v.Date,
			//	v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
			//);

		builder.HasOne(ep => ep.Participant)
			.WithMany(p => p.Events)
			.HasForeignKey(ep => ep.ParticipantId);

		builder.HasOne(ep => ep.Event)
			.WithMany(e => e.EventParticipants)
			.HasForeignKey(ep => ep.EventId);
	}
}
