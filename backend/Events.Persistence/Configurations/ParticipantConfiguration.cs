using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Persistence.Entities;

namespace Events.Persistence.Configurations;

public partial class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantEntity>
{
	public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
	{
		builder.HasKey(p => p.Id);

		builder.ToTable("Participant");

		builder
			.Property(e => e.FirstName)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.Property(e => e.LastName)
			.HasMaxLength(100)
			.IsRequired();

		builder
			.Property(e => e.DateOfBirth)
			.IsRequired();

		builder
			.Property(e => e.EventRegistrationDate)
			.IsRequired(false);

		builder
			.Property(e => e.Email)
			.HasMaxLength(100)
			.IsRequired();
	}
}