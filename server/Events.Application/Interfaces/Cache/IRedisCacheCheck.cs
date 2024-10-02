using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

namespace Events.Application.Common.Cache;

public interface IRedisCacheCheck
{
	public Task<IList<EventModel>> CheckImagesInCache(IList<Guid> ids);
	public Task<EventModel?> CheckImageInCache(Guid id);
}
