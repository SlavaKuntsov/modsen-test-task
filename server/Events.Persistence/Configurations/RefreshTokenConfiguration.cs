using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Events.Persistence.Entities;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
	public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
	{
		builder.HasKey(rt => rt.Id);

		builder.ToTable("RefreshToken");

		builder.Property(rt => rt.Token)
			   .IsRequired()
			   .IsConcurrencyToken(); // Защита от одновременных изменений

		builder.Property(rt => rt.CreatedAt)
			   .IsRequired();

		builder.Property(rt => rt.ExpiresAt)
			   .IsRequired();

		builder.Property(rt => rt.IsRevoked)
			   .IsRequired();

		builder.HasOne(rt => rt.Admin)
			   .WithOne(a => a.RefreshToken)
			   .HasForeignKey<RefreshTokenEntity>(rt => rt.AdminId)
			   .OnDelete(DeleteBehavior.Cascade); 

		builder.HasOne(rt => rt.Participant)
			   .WithOne(p => p.RefreshToken)
			   .HasForeignKey<RefreshTokenEntity>(rt => rt.UserId)
			   .OnDelete(DeleteBehavior.Cascade); 
	}
}
