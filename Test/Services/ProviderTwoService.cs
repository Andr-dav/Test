using Test.Models;
using TestTask;
using Route = Test.Models.Route;
namespace Test.Services
{
    public class ProviderTwoService : IProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlProviderTwoSearch;
        private readonly string _urlProviderTwoPing;
        private readonly ILogger<ProviderTwoService> _logger;
        private readonly IRouteMapperService _routeMapperService;
        public ProviderTwoService(HttpClient httpClient, IConfiguration configuration, ILogger<ProviderTwoService> logger, IRouteMapperService routeMapperService)
        {
            _httpClient = httpClient;
            _urlProviderTwoSearch = configuration.GetValue<string>("UrlProviderTwoSearch");
            _urlProviderTwoPing = configuration.GetValue<string>("UrlProviderTwoPing");
            _logger = logger;
            _routeMapperService = routeMapperService;
        }
        public async Task<List<Route>> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var providerRequest = new ProviderTwoSearchRequest
            {
                Departure = request.Origin,
                Arrival = request.Destination,
                DepartureDate = request.OriginDateTime,
                MinTimeLimit = request.Filters?.MinTimeLimit
            };
            if (!await IsAvailableAsync(cancellationToken))
            {
                _logger.LogWarning("Provider Two is not available.");
                return new List<Route>();
            }
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(_urlProviderTwoSearch, providerRequest, cancellationToken);
                httpResponse.EnsureSuccessStatusCode();
                var providerResponse = await httpResponse.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>(cancellationToken: cancellationToken);

                return _routeMapperService.MapProviderTwoSearchResponseToRoutes(providerResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling Provider Two search.");
                return new List<Route>();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Search operation was cancelled.");
                return new List<Route>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the search operation.");
                return new List<Route>();
            }
        }
        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlProviderTwoPing, cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider Two is unavailable due to an exception.");
                return false;
            }
        }
    }
}