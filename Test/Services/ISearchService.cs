using Test.Models;

namespace TestTask;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
    Task<ProviderAvailabilityStatus> IsAvailableAsync(CancellationToken cancellationToken);
}
