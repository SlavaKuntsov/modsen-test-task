using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Events;

public record CreateEventRequest(
	[Required]
	string Title,
	[Required]
	string Description,
	[Required]
	string EventDateTime,
	[Required]
	string Location,
	[Required]
	string Category,
	[Required]
	int MaxParticipants,
	[Required]
	int ParticipantsCount,
	[Required]
	byte[] Image);