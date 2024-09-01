using CSharpFunctionalExtensions;

using Events.Domain.Enums;

namespace Events.Domain.Models;

public class RefreshTokenModel
{
	public Guid Id { get; set; }

	public string Token { get; set; } = string.Empty;

	public DateTime ExpiresAt { get; set; }

	public bool IsRevoked { get; set; }

	public DateTime CreatedAt { get; set; }

	public Guid? AdminId { get; set; } // Идентификатор администратора, к которому привязан токен

	public Guid? UserId { get; set; } // Идентификатор участника, к которому привязан токен

	public RefreshTokenModel(Guid id, string token, DateTime expiresAt, bool isRevoked, DateTime createdAt, Guid? adminId, Guid? participantId)
	{
		Id = id;
		Token = token;
		ExpiresAt = expiresAt;
		IsRevoked = isRevoked;
		CreatedAt = createdAt;
		AdminId = adminId;
		UserId = participantId;
	}

	public static Result<RefreshTokenModel> Create(Guid userId, Role role, string token, int refreshTokenExpirationDays)
	{
		if (string.IsNullOrWhiteSpace(token))
			return Result.Failure<RefreshTokenModel>("Token cannot be empty");

		if (userId == Guid.Empty)
			return Result.Failure<RefreshTokenModel>("Invalid user ID");

		var refreshToken = new RefreshTokenModel
		(
			Guid.NewGuid(),
			token,
			DateTime.UtcNow.Add(TimeSpan.FromDays(refreshTokenExpirationDays)),
			false,
			DateTime.UtcNow,
			role == Role.Admin ? userId : null,
			role == Role.User ? userId : null
		);

		return Result.Success(refreshToken);
	}
}


