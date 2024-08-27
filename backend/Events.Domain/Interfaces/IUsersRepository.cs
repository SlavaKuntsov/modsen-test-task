using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Events.Domain.Models;

namespace Events.Domain.Interfaces;

public interface IUsersRepository
{
	public Task<ParticipantModel> Get(string email, string password);
	public Task<ParticipantModel> Get(string email);
	public Task<Guid> Create(ParticipantModel user);
}
