using Test.Models;

namespace Test.Services
{
    public interface IProviderService 
    {
        Task<List<Models.Route>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
