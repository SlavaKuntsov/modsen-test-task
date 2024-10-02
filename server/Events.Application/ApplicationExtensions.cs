using Events.Application.DTOs;
using Events.Application.Handlers.Tokens;
using Events.Application.Handlers.Users;
using Events.Application.Quries.Events;
using Events.Domain.Models.Users;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// Не работает
		// Регистрация MediatR и обработчиков
		//services.AddMediatR(cfg =>
		//{
		//	cfg.RegisterServicesFromAssemblyContaining<UserRegistrationCommandHandler<ParticipantModel>>(); 
		//	cfg.RegisterServicesFromAssemblyContaining<UserRegistrationCommandHandler<AdminModel>>(); 
		//});

		services.AddTransient<IRequestHandler<DeleteUserCommand<ParticipantModel>>, DeleteUserCommandHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<DeleteUserCommand<AdminModel>>, DeleteUserCommandHandler<AdminModel>>();

		services.AddTransient<IRequestHandler<GetOrAuthorizeUserQuery<ParticipantModel>, ParticipantModel?>, GetOrAuthorizeUserQueryHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<GetOrAuthorizeUserQuery<AdminModel>, AdminModel?>, GetOrAuthorizeUserQueryHandler<AdminModel>>();

		services.AddTransient<IRequestHandler<GetUserByFilterQuery<ParticipantModel>, ParticipantModel?>, GetUserByFilterQueryHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<GetUserByFilterQuery<AdminModel>, AdminModel?>, GetUserByFilterQueryHandler<AdminModel>>();

		services.AddTransient<IRequestHandler<GetUsersQuery<ParticipantModel>, IList<ParticipantModel>>, GetUsersQueryHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<GetUsersQuery<AdminModel>, IList<AdminModel>>, GetUsersQueryHandler<AdminModel>>();

		services.AddTransient<IRequestHandler<LoginUserQuery<ParticipantModel>>, LoginUserCommandHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<LoginUserQuery<AdminModel>>, LoginUserCommandHandler<AdminModel>>();

		services.AddTransient<IRequestHandler<UpdateParticipantCommand, ParticipantDto>, UpdateParticipantCommandHandler>();

		services.AddTransient<IRequestHandler<UserRegistrationCommand<ParticipantModel>, AuthDto>, UserRegistrationCommandHandler<ParticipantModel>>();
		services.AddTransient<IRequestHandler<UserRegistrationCommand<AdminModel>, AuthDto>, UserRegistrationCommandHandler<AdminModel>>();


		return services;
	}
}
