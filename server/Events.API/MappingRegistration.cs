using Mapster;

namespace Events.API;

public class MappingRegistration : IRegister
{
	void IRegister.Register(TypeAdapterConfig config)
	{
		//config.NewConfig<EventEntity, EventModel>();
		//config.NewConfig<EventModel, GetEventResponse>();
	}
}
