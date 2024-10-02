using Events.Application.Common.Cache;
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
		try
		{
			var qwe = await _cache.GetAsync(key);
			return qwe;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error retrieving image from cache for key {key}: {ex.Message}");
		}
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