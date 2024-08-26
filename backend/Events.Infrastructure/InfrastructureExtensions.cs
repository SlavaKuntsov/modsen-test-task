using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		//services.AddScoped<IJwtProvider, JwtProvider>();
		//services.AddScoped<IPasswordHasher, PasswordHasher>();

		return services;
	}
}
