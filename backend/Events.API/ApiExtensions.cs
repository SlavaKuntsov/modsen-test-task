using System.Reflection;

using Mapster;

using MapsterMapper;

namespace Events.API;

public static class ApiExtensions
{
	public static IServiceCollection AddAPI(this IServiceCollection services)
	{
		var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
		typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
		var mapperConfig = new Mapper(typeAdapterConfig);
		services.AddSingleton<IMapper>(mapperConfig);

		return services;
	}
}