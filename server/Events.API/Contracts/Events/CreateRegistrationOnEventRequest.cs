using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Events;

public record CreateRegistrationOnEventRequest(
	[Required]
	 string EventId,
	[Required]
	string ParticipantId
	);

