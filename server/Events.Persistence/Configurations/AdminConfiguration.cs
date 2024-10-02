using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Persistence.Entities;

namespace Events.Persistence.Configurations;

public partial class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
{
	public void Configure(EntityTypeBuilder<AdminEntity> builder)
	{
		builder.ToTable("Admin");

		builder.HasKey(a => a.Id);

		builder.Property(a => a.Email)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(a => a.Password)
			.IsRequired();

		builder.Property(a => a.IsActiveAdmin)
			.IsRequired();

		builder.HasOne(a => a.RefreshToken)
			   .WithOne(rt => rt.Admin)
			   .HasForeignKey<RefreshTokenEntity>(rt => rt.AdminId);

		builder.HasData(
			new AdminEntity
			{
				Id = Guid.NewGuid(),
				Email = "user@example.com",
				Password = BCrypt.Net.BCrypt.EnhancedHashPassword("qweQWE123"),
				Role = Domain.Enums.Role.Admin,
				IsActiveAdmin = true
			});
	}
}