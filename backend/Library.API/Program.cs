using Library.Application;
using Library.Infrastructure;
using Library.Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSwaggerGen();
services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

services
	.AddPersistence(configuration)
	.AddApplication()
	.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();