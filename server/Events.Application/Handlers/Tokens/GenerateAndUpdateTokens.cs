using Events.Application.Common.Auth;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;
using Events.Application.DTOs;
using MediatR;
using Events.Domain.Enums;
namespace Events.Application.Handlers.Tokens;

public class GenerateAndUpdateTokensCommand(Guid id, Role role) : IRequest<AuthDto>
{
	public Guid Id { get; private set; } = id;
	public Role Role { get; private set; } = role;
}

public class GenerateAndUpdateTokensCommandHandler(ITokensRepository tokensRepository, IJwt jwt) : IRequestHandler<GenerateAndUpdateTokensCommand, AuthDto>
{
	private readonly ITokensRepository _tokensRepository = tokensRepository;
	private readonly IJwt _jwt = jwt;

	public async Task<AuthDto> Handle(GenerateAndUpdateTokensCommand request, CancellationToken cancellationToken)
	{
		var accessToken = _jwt.GenerateAccessToken(request.Id, request.Role);
		var newRefreshToken = _jwt.GenerateRefreshToken();

		var refreshTokenModel = RefreshTokenModel.Create(request.Id,
												   request.Role,
												   newRefreshToken,
												   _jwt.GetRefreshTokenExpirationDays());

		await _tokensRepository.UpdateRefreshToken(request.Id, request.Role, refreshTokenModel.Value, cancellationToken);

		return new AuthDto
		{
			AccessToken = accessToken,
			RefreshToken = newRefreshToken
		};

	}
}