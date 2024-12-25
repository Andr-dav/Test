using Test.Models;

namespace Test.Services
{
    public interface ICacheService
    {
        void SetCache(SearchResponse response);
        SearchResponse GetCache();
    }
}
