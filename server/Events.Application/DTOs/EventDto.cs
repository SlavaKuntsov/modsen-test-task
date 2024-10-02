namespace Events.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EventDateTime { get; set; }

    public string Location { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int MaxParticipants { get; set; }

    public int ParticipantsCount { get; set; }

    public string Image { get; set; } = string.Empty;
}