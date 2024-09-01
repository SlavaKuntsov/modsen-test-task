﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Persistence.Entities;

namespace Events.Persistence.Configurations;

public partial class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantEntity>
{
	public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
	{
		builder.ToTable("Participant"); // Таблица для участников

		builder.HasKey(p => p.Id); // Уникальный ключ

		builder.Property(p => p.Email)
			.HasMaxLength(100)
			.IsRequired(); // Обязательное поле

		builder.Property(p => p.Password)
			.IsRequired(); // Обязательное поле

		builder.Property(p => p.Role)
			.IsRequired(); // Обязательное поле

		builder.Property(p => p.FirstName)
			.HasMaxLength(100)
			.IsRequired(); // Обязательное поле

		builder.Property(p => p.LastName)
			.HasMaxLength(100)
			.IsRequired(); // Обязательное поле

		builder.Property(p => p.DateOfBirth)
			.IsRequired() // Обязательное поле
			.HasConversion(
				v => v.ToUniversalTime(), // Преобразование к UTC перед сохранением
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // Преобразование к UTC при загрузке;

		builder.Property(p => p.EventRegistrationDate)
			.IsRequired(false); // Необязательное поле

		// Настройка связи с RefreshTokenEntity (один-к-одному)
		builder.HasOne(p => p.RefreshToken)
			   .WithOne(rt => rt.Participant)
			   .HasForeignKey<RefreshTokenEntity>(rt => rt.UserId); // Внешний ключ
	}
}
