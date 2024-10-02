using Events.API.Middlewares;

namespace Library.API.Extensions;

public static class ExceptionHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseCustomExceptionHandler(this
		IApplicationBuilder builder)
	{
		return builder.UseMiddleware<GlobalExceptionHandler>();
	}
}
