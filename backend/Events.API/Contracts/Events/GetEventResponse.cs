using Events.Domain.Models;

namespace Events.API.Contracts.Events;

public class GetEventResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EventDateTime { get; set; }

    public string Location { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int MaxParticipants { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public IList<ParticipantModel> Participants { get; set; } = [];
}
