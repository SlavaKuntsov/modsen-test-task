using Events.Domain.Models;

namespace Events.Application.Auth;

public interface IJwt
{
	public string Generate(ParticipantModel user);
}