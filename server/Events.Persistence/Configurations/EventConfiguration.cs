using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Persistence.Entities;

namespace Events.Persistence.Configurations;

public partial class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
	public void Configure(EntityTypeBuilder<EventEntity> builder)
	{
		builder.HasKey(e => e.Id);

		builder.ToTable("Event");

		builder.Property(e => e.Title)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(e => e.Description)
			.IsRequired();

		builder.Property(e => e.EventDateTime)
			.IsRequired()
			.HasColumnType("date")
			.HasConversion(
				v => v.Date,
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
			);

		builder.Property(e => e.Location)
			.IsRequired();

		builder.Property(e => e.Category)
			.IsRequired();

		builder.Property(e => e.MaxParticipants)
			.IsRequired();

		builder.Property(e => e.ImageUrl)
			.IsRequired(false);

		// Настройка связи с участниками через EventParticipant
		builder.HasMany(e => e.EventParticipants)
			.WithOne(ep => ep.Event)
			.HasForeignKey(ep => ep.EventId);
	}
}
