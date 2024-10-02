using Events.Domain.Interfaces.Repositories;
using Events.Domain.Models;

namespace Events.Application.Common.Cache;

public interface IRedisCacheCheck
{
	public Task<IList<EventModel>> CheckImagesInCache(IList<Guid> ids, CancellationToken cancellationToken);
	public Task<EventModel?> CheckImageInCache(Guid id, CancellationToken cancellationToken);
}
