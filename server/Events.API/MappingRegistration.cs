using Events.Application.DTOs;
using Events.Application.Handlers.Users;
using Events.Domain.Models;
using Events.Domain.Models.Users;

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

		TypeAdapterConfig<UpdateParticipantCommand, ParticipantModel>
			.NewConfig()
			.Map(dest => dest.FirstName, src => src.FirstName)
			.Map(dest => dest.LastName, src => src.LastName)
			.Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
			.Ignore(dest => dest.Email) // Email не изменяем
			.Ignore(dest => dest.Password) // Password не изменяем
			.Ignore(dest => dest.Role); // Role не изменяем
	}
}
