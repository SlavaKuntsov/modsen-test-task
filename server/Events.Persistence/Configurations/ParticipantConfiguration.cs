using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistence.Configurations;

public partial class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantEntity>
{
	public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
	{
		builder.ToTable("Participant");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Email)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.Password)
			.IsRequired();

		//builder.Property(p => p.Role)
		//	.IsRequired();

		builder.Property(p => p.FirstName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.LastName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.DateOfBirth)
			.IsRequired()
			.HasColumnType("date")
			.HasConversion(
				v => v.Date,
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
			);

		builder.HasOne(p => p.RefreshToken)
			.WithOne(rt => rt.Participant)
			.HasForeignKey<RefreshTokenEntity>(rt => rt.UserId);
	}
}
