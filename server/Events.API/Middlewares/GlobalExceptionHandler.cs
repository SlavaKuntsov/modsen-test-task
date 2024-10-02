using Events.Application.Exceptions;

using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
	public GlobalExceptionHandler()
	{
	}

	private static readonly Dictionary<Type, (int StatusCode, string Title)> ExceptionMappings = new()
	{
		{ typeof(UnauthorizedAccessException), (StatusCodes.Status401Unauthorized, "Unauthorized Access") },
		{ typeof(ArgumentException), (StatusCodes.Status400BadRequest, "Invalid Argument") },
		{ typeof(NullReferenceException), (StatusCodes.Status500InternalServerError, "Null Reference Error") },
		{ typeof(InvalidOperationException), (StatusCodes.Status409Conflict, "Invalid Operation") },
		{ typeof(NotFoundException), (StatusCodes.Status404NotFound, "Not Found") },
		{ typeof(ValidationException), (StatusCodes.Status400BadRequest, "Invalid Data") },
		{ typeof(RegistrationExistsException), (StatusCodes.Status400BadRequest, "Bad Request") },
		{ typeof(DeleteException), (StatusCodes.Status400BadRequest, "Cannot Delete") },
	};

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var (statusCode, title) = ExceptionMappings.TryGetValue(exception.GetType(), out var mapping)
			? mapping
			: (StatusCodes.Status500InternalServerError, "Internal Server Error");

		var problemDetails = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = $"{exception.Message}"
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		httpContext.Response.ContentType = "application/json";

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
}
