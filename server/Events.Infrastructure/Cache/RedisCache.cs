using Events.Application.Cache;

using Microsoft.Extensions.Caching.Distributed;

namespace Events.Infrastructure.Cache;

public class RedisCache : IRedisCache
{
	private readonly IDistributedCache _cache;

	public RedisCache(IDistributedCache cache)
	{
		_cache = cache;
	}

	public async Task<byte[]?> GetImage(string key)
	{
		return await _cache.GetAsync(key);
	}

	public async Task SetImage(string key, byte[] image, TimeSpan expiration)
	{
		var options = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = expiration
		};
		await _cache.SetAsync(key, image, options);
	}
}