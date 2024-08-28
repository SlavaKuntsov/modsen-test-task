using Events.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistence.Configurations;

public partial class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
	public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
	{
		builder.HasKey(rt => rt.Id);

		builder.ToTable("RefreshToken");

		// Указываем, что свойство Token обязательно для заполнения
		builder.Property(rt => rt.Token)
			   .IsRequired()
			   .IsConcurrencyToken(); // Добавляем защиту от одновременных изменений

		builder.Property(rt => rt.CreatedAt)
			   .IsRequired();

		builder.Property(rt => rt.ExpiresAt)
			   .IsRequired();

		builder.Property(rt => rt.IsRevoked)
			   .IsRequired();

		// Устанавливаем отношение с сущностью пользователя (один ко многим)
		builder.HasOne(rt => rt.User)
			   .WithMany(p => p.RefreshTokens)
			   .HasForeignKey(rt => rt.UserId)
			   .OnDelete(DeleteBehavior.Cascade); // Удаляем токены, если удаляется пользователь
	}
}
