using Events.Domain.Models;

namespace Events.Application.Common.Cache;

public interface IRedisCache
{
    public Task<byte[]?> GetImage(string key);

    public Task SetImage(string key, byte[] image, TimeSpan expiration);
}
