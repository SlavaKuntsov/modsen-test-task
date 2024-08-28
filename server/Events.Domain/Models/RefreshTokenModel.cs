using CSharpFunctionalExtensions;

namespace Events.Domain.Models;

public class RefreshTokenModel
{
	public Guid Id { get; set; }

	public string Token { get; set; } = string.Empty;

	public DateTime ExpiresAt { get; set; }

	public bool IsRevoked { get; set; }

	public DateTime CreatedAt { get; set; }

	public Guid UserId { get; set; }

	public RefreshTokenModel(Guid id, string token, DateTime expiresAt, bool isRevoked, DateTime createdAt, Guid userId)
	{
		Id = id;
		Token = token;
		ExpiresAt = expiresAt;
		IsRevoked = isRevoked;
		CreatedAt = createdAt;
		UserId = userId;
	}

	public static Result<RefreshTokenModel> Create(Guid userId, string token, int refreshTokenExpirationDays)
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
			userId
		);

		return Result.Success(refreshToken);
	}
}


