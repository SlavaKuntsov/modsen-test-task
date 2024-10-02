using System.Diagnostics;
using System.Reflection;
using System.Text;

using Events.API.Behaviors;
using Events.API.Middlewares;
using Events.API.Validators;
using Events.Application.Handlers.Events;
using Events.Application.Handlers.Users;
using Events.Domain.Models.Users;
using Events.Infrastructure.Auth;

using FluentValidation;

using Mapster;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Events.API.Extensions;

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
						context.Token = context.Request.Cookies[COOKIE_NAME];
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
				policy.WithOrigins("http://localhost:3000");
				policy.WithOrigins("http://localhost:5000");
				policy.AllowAnyHeader();
				policy.AllowAnyMethod();
				policy.AllowCredentials();
			});
		});


		services.AddAuthorizationBuilder()
			.AddPolicy("AdminOnly", policy =>
			{
				policy.RequireRole("Admin");
				policy.AddRequirements(new ActiveAdminRequirement());
			})
			.AddPolicy("UserOrAdmin", policy =>
			{
				policy.RequireRole("User", "Admin");
				policy.AddRequirements(new ActiveAdminRequirement());
			})
			.AddPolicy("UserOnly", policy => policy.RequireRole("User"));


		services.AddScoped<IAuthorizationHandler, ActiveAdminHandler>();

		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = configuration.GetConnectionString("RedisCache");
		});

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblyContaining<ActiveAdminHandler>(); // Обязательно добавьте обработчик здесь
			cfg.RegisterServicesFromAssemblyContaining<GetUserByFilterQueryHandler<ParticipantModel>>();
		});

		services.AddValidatorsFromAssemblyContaining<BaseCommandValidator<CreateEventCommand>>();

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


		return services;
	}
}