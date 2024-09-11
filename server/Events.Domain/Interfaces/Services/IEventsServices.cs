﻿using CSharpFunctionalExtensions;

using Events.Domain.Models;
using Events.Domain.Models.Users;

namespace Events.Domain.Interfaces.Services;

public interface IEventsServices
{
	public Task<IList<EventModel>> Get();
	public Task<IList<EventModel>> GetWithoutImage();
	public Task<Result<EventModel>> Get(Guid id);
	public Task<Result<IList<EventModel>>> GetByParticipantId(Guid id);
	public Task<Result<IList<EventModel>>> GetByTitle(string title);
	public Task<Result<IList<EventModel>>> GetByLocation(string title);
	public Task<Result<IList<EventModel>>> GetByCategory(string title);
	public Task<Result<IList<ParticipantModel>>> GetEventParticipants(Guid eventId);

	public Task<Result<Guid>> Create(string title, string description, string eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl);

	public Task<Result> AddParticipantToEvent(string eventId, string participantId);

	public Task<Result> RemoveParticipantFromEvent(string eventId, string participantId);

	public Task<Result<Guid>> Update(Guid id, string title, string description, string eventDateTime, string location, string category, int maxParticipants, byte[] imageUrl);

	public Task<Result> Delete(Guid eventId);
}
