using System.ComponentModel.DataAnnotations;

public record UpdateEventRequest(
	[Required]
	Guid Id,
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
	//[Required]
	//public string ImageUrl { get; set; } = string.Empty;
	[Required]
	 byte[] Image);
