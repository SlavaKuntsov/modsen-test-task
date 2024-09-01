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

		builder
			.Property(e => e.Title)
			.HasMaxLength(200)
			.IsRequired();

		builder
			.Property(e => e.Description)
			.IsRequired();

		builder.Property(p => p.EventDateTime)
			.IsRequired() // Обязательное поле
			.HasColumnType("date") // Указываем тип колонки как date
			.HasConversion(
				v => v.Date, // Сохраняем только дату
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Указываем, что дата - это UTC
			);

		builder
			.Property(e => e.Location)
			.IsRequired();

		builder
			.Property(e => e.Category)
			.IsRequired();

		builder
			.Property(e => e.MaxParticipants)
			.IsRequired();

		builder
			.Property(e => e.ImageUrl)
			.IsRequired(false);

		builder
			.Property(e => e.MaxParticipants)
			.IsRequired();

		builder.HasMany(e => e.Participants)
			.WithMany(p => p.Events)
			.UsingEntity<EventParticipantEntity>(
				//"EventParticipant",
				l =>
				l.HasOne<ParticipantEntity>()
					.WithMany()
					.HasForeignKey(p => p.ParticipantId),
				r =>
				r.HasOne<EventEntity>()
					.WithMany()
					.HasForeignKey(e => e.EventId));
	}
}