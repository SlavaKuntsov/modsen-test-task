using Events.Application.Auth;
using Events.Domain.Enums;

namespace Events.Persistence.Entities;

public class ParticipantEntity : IUser
{
	public Guid Id { get; set; } // уникальный идентификатор

	public string Email { get; set; } = string.Empty; // почта

	public string Password { get; set; } = string.Empty; // пароль

	public Role Role { get; set; } // роль

	public string FirstName { get; set; } = string.Empty; // имя

	public string LastName { get; set; } = string.Empty; // фамилия

	public DateTime DateOfBirth { get; set; } // дата рождения

	public DateTime? EventRegistrationDate { get; set; } = null; // дата регистрации на событие

	public IList<EventEntity> Events { get; set; } = new List<EventEntity>(); // связанные события

	public RefreshTokenEntity RefreshToken { get; set; }
}
