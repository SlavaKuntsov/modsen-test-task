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

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
											 Exception exception,
											 CancellationToken cancellationToken)
	{
		ProblemDetails problemDetails = new();

		switch (exception)
		{
			case UnauthorizedAccessException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status401Unauthorized,
					Title = "Unauthorized Access",
					Detail = $"Exception occurred: {exception.Message}"
				};
				break;

			case ArgumentException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Invalid Argument",
					Detail = $"Invalid argument: {exception.Message}"
				};
				break;

			case NullReferenceException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status500InternalServerError,
					Title = "Null Reference Error",
					Detail = $"A null reference occurred: {exception.Message}"
				};
				break;

			case InvalidOperationException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status409Conflict,
					Title = "Invalid Operation",
					Detail = $"Invalid operation: {exception.Message}"
				};
				break;

			case NotFoundException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status404NotFound,
					Title = "Not Found",
					Detail = $"{exception.Message}"
				};
				break;

			case ValidationException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Invalid Data",
					Detail = $"Invalid data: {exception.Message}"
				};
				break;

			case RegistrationExistsException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Bad Request",
					Detail = $"{exception.Message}"
				};
				break;

			default:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status500InternalServerError,
					Title = "Internal Server Error",
					Detail = $"An unexpected error occurred: {exception.Message}"
				};
				break;
		}

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		httpContext.Response.ContentType = "application/json";

		await httpContext.Response
			.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
}
