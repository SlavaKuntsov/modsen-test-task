using CSharpFunctionalExtensions;

using Events.Domain.Enums;
using Events.Domain.Validators.Users;

namespace Events.Domain.Models;

public class RefreshTokenModel
{
	public Guid Id { get; private set; }

	public string Token { get; private set; } = string.Empty;

	public DateTime ExpiresAt { get; private set; }

	public bool IsRevoked { get; private set; }

	public DateTime CreatedAt { get; private set; }

	public Guid? AdminId { get; private set; }

	public Guid? UserId { get; private set; }

	public RefreshTokenModel() { }

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
		RefreshTokenModel model = new(
			Guid.NewGuid(),
			token,
			DateTime.UtcNow.Add(TimeSpan.FromDays(refreshTokenExpirationDays)),
			false,
			DateTime.UtcNow,
			role == Role.Admin ? userId : null,
			role == Role.User ? userId : null);

		var validator = new RefreshTokenModelValidator();
		var validationResult = validator.Validate(model);

		if (!validationResult.IsValid)
			return Result.Failure<RefreshTokenModel>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

		return model;
	}
}


