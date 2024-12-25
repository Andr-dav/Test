using Microsoft.Extensions.Caching.Memory;
using Test.Models;
namespace Test.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly string _cacheKey;
        private readonly int _cacheDurationMinutes;
        public CacheService(IMemoryCache cache, ILogger<CacheService> logger,
            IConfiguration configuration)
        {
            _cacheKey = configuration.GetValue<string>("CacheKey");
            _cacheDurationMinutes = configuration.GetValue<int>("CacheTimeMin");
            _logger = logger;
            _cache = cache;
        }
        public void SetCache(SearchResponse response)
        {
            _cache.Set(_cacheKey, response, TimeSpan.FromMinutes(_cacheDurationMinutes));
            _logger.LogInformation("Cache set for key: {CacheKey}", _cacheKey);
        }
        public SearchResponse GetCache()
        {
            if (_cache.TryGetValue(_cacheKey, out SearchResponse cachedResponse))
            {
                _logger.LogInformation("Returning cached response for key: {CacheKey}", _cacheKey);
                return cachedResponse;
            }
            _logger.LogInformation("No cached response found for key: {CacheKey}", _cacheKey);
            return null;
        }
    }
}