using Events.Domain.Models;

namespace Events.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    public Task<ParticipantModel> Get(string email);
    public Task<ParticipantModel> Get(string email, string password);

    public Task<Guid> Create(ParticipantModel user);
}
