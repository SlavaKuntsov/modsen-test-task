using Events.Persistence;
using Events.Application;
using Events.Infrastructure;
using Events.Domain.Interfaces;
using Events.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services  = builder.Services;
var configuration = builder.Configuration;

services.AddSwaggerGen();
services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

services
	.AddApplication()
	.AddInfrastructure()
	.AddPersistence(configuration);

services
	.AddScoped<IEventRepository, EventRepository>();

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
