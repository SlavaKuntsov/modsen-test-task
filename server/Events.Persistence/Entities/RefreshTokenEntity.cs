namespace Events.Persistence.Entities;

public class RefreshTokenEntity
{
	public Guid Id { get; set; } // Уникальный идентификатор для каждого токена

	public string Token { get; set; } = string.Empty; // Сам refresh token

	public DateTime ExpiresAt { get; set; } // Дата истечения срока действия токена

	public bool IsRevoked { get; set; } // Флаг, показывающий, был ли токен отозван

	public DateTime CreatedAt { get; set; } // Дата создания токена

	//public Guid UserId { get; set; } // Идентификатор пользователя, к которому привязан токен

	//public virtual IUser User { get; set; } // Общее свойство для связи с пользователем

	public Guid? AdminId { get; set; } // Идентификатор администратора, к которому привязан токен
	public Guid? UserId { get; set; } // Идентификатор участника, к которому привязан токен

	// Навигационные свойства
	public virtual AdminEntity Admin { get; set; } // Убедитесь, что связь виртуальная
	public virtual ParticipantEntity Participant { get; set; } // Убедитесь, что связь виртуальная
}
