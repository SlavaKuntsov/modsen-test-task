using Events.Persistence;
using Events.Application;
using Events.Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Events.API.Middlewares;
using Events.API.Extensions;

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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
}
app.ApplyMigrations();

app.UseExceptionHandler();

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
