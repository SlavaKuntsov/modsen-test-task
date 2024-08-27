using Events.Persistence;
using Events.Application;
using Events.Infrastructure;
using Events.Domain.Interfaces;
using Events.Persistence.Repositories;
using Mapster;
using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services  = builder.Services;
var configuration = builder.Configuration;

services.AddSwaggerGen();
services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
var mapperConfig = new Mapper(typeAdapterConfig);
services.AddSingleton<IMapper>(mapperConfig);

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
