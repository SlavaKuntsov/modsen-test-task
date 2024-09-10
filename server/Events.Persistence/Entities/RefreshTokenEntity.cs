namespace Events.Persistence.Entities;

public class RefreshTokenEntity
{
	public Guid Id { get; set; } 

	public string Token { get; set; } = string.Empty; 

	public DateTime ExpiresAt { get; set; } 

	public bool IsRevoked { get; set; } 

	public DateTime CreatedAt { get; set; } 

	public Guid? AdminId { get; set; } 
	public Guid? UserId { get; set; } 

	public virtual AdminEntity Admin { get; set; } 
	public virtual ParticipantEntity Participant { get; set; } 
}
