using Events.Persistence;
using Events.Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Events.API.Middlewares;
using Events.API.Extensions;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var services  = builder.Services;
var configuration = builder.Configuration;

services.AddSwaggerGen();
services.AddControllers();

services.AddExceptionHandler<GlobalExceptionHandler>();

services.AddProblemDetails();

services
	.AddAPI(configuration)
	.AddApplication()
	.AddInfrastructure()
	.AddPersistence(configuration);

var app = builder.Build();

app.UseExceptionHandler(config =>
{
	config.Run(async context =>
	{
		var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
		var exception = exceptionHandler?.Error;

		if (exception != null)
		{
			var handler = app.Services.GetRequiredService<GlobalExceptionHandler>();
			await handler.TryHandleAsync(context, exception, CancellationToken.None);
		}
	});
});

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.ApplyMigrations();

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.None,
	HttpOnly = HttpOnlyPolicy.Always,
	Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();
app.UseCors();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
