using Events.Application.DTOs;
using Events.Domain.Models;

using Mapster;

namespace Events.Application.Common.Mappings;

public class EventMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<EventModel, EventDto>()
			.Map(dest => dest.Image, src => src.Image != null && src.Image.Length > 0
				? Convert.ToBase64String(src.Image)
				: string.Empty);
	}
}
