using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Persistence.Entities;

namespace Events.Persistence.Configurations;

public partial class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
{
	public void Configure(EntityTypeBuilder<AdminEntity> builder)
	{
		builder.ToTable("Admin"); // Таблица для администраторов

		builder.HasKey(a => a.Id); // Уникальный ключ

		builder.Property(a => a.Email)
			.HasMaxLength(100)
			.IsRequired(); // Обязательное поле

		builder.Property(a => a.Password)
			.IsRequired(); // Обязательное поле

		builder.Property(a => a.Role)
			.IsRequired(); // Обязательное поле

		builder.Property(a => a.IsActiveAdmin)
			.IsRequired(); // Обязательное поле

		// Настройка связи с RefreshTokenEntity (один-к-одному)
		builder.HasOne(a => a.RefreshToken)
			   .WithOne(rt => rt.Admin)
			   .HasForeignKey<RefreshTokenEntity>(rt => rt.AdminId); // Внешний ключ
	}
}