using Test.Models;
using TestTask;
using Route = Test.Models.Route;
namespace Test.Services
{
    public class ProviderOneService : IProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlProviderOneSearch;
        private readonly string _urlProviderOnePing;
        private readonly ILogger<ProviderOneService> _logger;
        private readonly IRouteMapperService _routeMapperService;
        public ProviderOneService(HttpClient httpClient, IConfiguration configuration, ILogger<ProviderOneService> logger, IRouteMapperService routeMapperService)
        {
            _httpClient = httpClient;
            _urlProviderOneSearch = configuration.GetValue<string>("UrlProviderOneSearch");
            _urlProviderOnePing = configuration.GetValue<string>("UrlProviderOnePing");
            _logger = logger;
            _routeMapperService = routeMapperService;
        }
        public async Task<List<Route>> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var providerRequest = new ProviderOneSearchRequest
            {
                From = request.Origin,
                To = request.Destination,
                DateFrom = request.OriginDateTime
            };

            if (!await IsAvailableAsync(cancellationToken))
            {
                _logger.LogWarning("Provider One is not available.");
                return new List<Route>();
            }
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(_urlProviderOneSearch, providerRequest, cancellationToken);
                httpResponse.EnsureSuccessStatusCode();
                var providerResponse = await httpResponse.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken);
                return _routeMapperService.MapProviderOneSearchResponseToRoutes(providerResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling Provider One search.");
                return new List<Route>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return new List<Route>();
            }
        }
       
        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlProviderOnePing, cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider One is unavailable due to an exception.");
                return false;
            }
        }
    }
}