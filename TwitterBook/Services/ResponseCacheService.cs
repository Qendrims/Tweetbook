using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace TwitterBook.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response == null)
        {
            return;
        }

        var serializeResponse = JsonConvert.SerializeObject(response);
        await _distributedCache.SetStringAsync(cacheKey,serializeResponse,new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });
    }

    public async Task<string> GetCachedResponseAsync(string cacheKey)
    {
        var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
    }
}