using Events.Application.Auth;
using Events.Application.Interfaces.Auth;
using Events.Infrastructure.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHash, PasswordHash>();
		services.AddScoped<IJwt, Jwt>();

		return services;
	}
}
