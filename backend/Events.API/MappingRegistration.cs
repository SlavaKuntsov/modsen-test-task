using Events.API.Contracts;
using Events.Domain.Models;
using Events.Persistence.Entities;

using Mapster;

namespace Events.API;

public class MappingRegistration : IRegister
{
	void IRegister.Register(TypeAdapterConfig config)
	{
		config.NewConfig<EventEntity, EventModel>();
		config.NewConfig<EventModel, GetEventResponse>();
	}
}
