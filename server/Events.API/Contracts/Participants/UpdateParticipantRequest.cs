using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Participants;

public record UpdateParticipantRequest(
	[Required]
	 Guid Id,
	[Required]
	 string FirstName,
	[Required]
	 string LastName,
	[Required]
	 string DateOfBirth);
