namespace Events.Application.Cache;

public interface IRedisCache
{
	public Task<byte[]?> GetImage(string key);

	public Task SetImage(string key, byte[] image, TimeSpan expiration);
}
