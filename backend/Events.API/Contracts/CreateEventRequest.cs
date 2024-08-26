using System.ComponentModel.DataAnnotations;

namespace Events.API.Contracts;

public class CreateEventRequest
{
	[Required]
	public string Title { get; set; }
}
