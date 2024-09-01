using CSharpFunctionalExtensions;

using Events.Domain.Models;

namespace Events.Domain.Interfaces.Services;

public interface IEventsServices
{
	public Task<IList<EventModel>> Get();
	public Task<Result<EventModel>> Get(Guid id);

	public Task<Result<Guid>> Create(Guid id, string title, string description, string eventDateTime, string Location, string category, int maxParticipants, string imageUrl);
}
