using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts.Events;

public class UpdateEventRequest
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Title { get; set; } = string.Empty;

	[Required]
	public string Description { get; set; } = string.Empty;

	[Required]
	public string EventDateTime { get; set; }

	[Required]
	public string Location { get; set; } = string.Empty;

	[Required]
	public string Category { get; set; } = string.Empty;

	[Required]
	public int MaxParticipants { get; set; }

	[Required]
	public int ParticipantsCount { get; set; }

	[Required]
	public string ImageUrl { get; set; } = string.Empty;
}
