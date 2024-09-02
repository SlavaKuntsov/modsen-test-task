using System.Diagnostics;
using System.Reflection;
using System.Text;

using Events.Infrastructure.Auth;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Events.API;

public static class ApiExtensions
{
	public const string COOKIE_NAME = "yummy-cackes";

	public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
	{
		var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
		typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

		var mapperConfig = new Mapper(typeAdapterConfig);
		services.AddSingleton<IMapper>(mapperConfig);

		var jwtOptions = configuration.GetSection(nameof(JwtModel)).Get<JwtModel>();

		services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.RequireHttpsMetadata = true;
				options.SaveToken = true;
				options.TokenValidationParameters = new()
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
				};
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						Debug.WriteLine("Authentication failed: " + context.Exception.Message);
						return Task.CompletedTask;
					},
					OnTokenValidated = context =>
					{
						Debug.WriteLine("Token is valid.");
						return Task.CompletedTask;
					},
					OnMessageReceived = context =>
					{
						context.Token = context.Request.Cookies[COOKIE_NAME]; // если токен передается в cookie
						return Task.CompletedTask;
					}
				};
			});

		services.Configure<JwtModel>(configuration.GetSection(nameof(JwtModel)));
		services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.WithOrigins("http://localhost:5173");
				policy.WithOrigins("http://localhost:5174");
				policy.AllowAnyHeader();
				policy.AllowAnyMethod();
				policy.AllowCredentials();
			});
		});

		services.AddAuthorization(options =>
		{
			options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

			options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

			options.AddPolicy("UserOrAdmin", policy =>
				policy.RequireRole("User", "Admin"));
		});

		return services;
	}
}