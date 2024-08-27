using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Events.Domain.Models;

namespace Events.Domain.Interfaces;

public interface IEventRepository
{
	public Task<ICollection<EventModel>> GetEvents();
	public Task<Guid> Create(EventModel eventModel);
}
