using Events.Application.Auth;
using Events.Application.Cache;
using Events.Application.Interfaces.Auth;
using Events.Infrastructure.Auth;
using Events.Infrastructure.Cache;

using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHash, PasswordHash>();
		services.AddScoped<IJwt, Jwt>();

		services.AddScoped<IRedisCache, RedisCache>();

		return services;
	}
}
