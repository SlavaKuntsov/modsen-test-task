using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Events;

public class CreateRegistrationOnEventRequest
{
	[Required]
	public string EventId { get; set; } = string.Empty;

	[Required]
	public string ParticipantId { get; set; } = string.Empty;
}
