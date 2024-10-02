using Events.Application.Common.Auth;
using Events.Application.Common.Cache;
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
		services.AddScoped<IRedisCacheCheck, RedisCacheCheck>();

		return services;
	}
}
