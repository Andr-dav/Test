using Test.Models;
using TestTask;
using Route = Test.Models.Route;

namespace Test.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProviderService _providerOneService;
        private readonly IProviderService _providerTwoService;
        private readonly IRouteFilterService _routeFilterService;
        private readonly ICacheService _cacheService;
        private readonly IRouteStatisticsService _routeStatisticsService;

        public SearchService(
            IProviderService providerOneService,
            IProviderService providerTwoService,
            IRouteFilterService routeFilterService,
            ICacheService cacheService,
            IRouteStatisticsService routeStatisticsService)
        {
            _providerOneService = providerOneService;
            _providerTwoService = providerTwoService;
            _routeFilterService = routeFilterService;
            _cacheService = cacheService;
            _routeStatisticsService = routeStatisticsService;
        }
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            List<Route> allRoutes = new List<Route>();
            SearchResponse response;
            if (request.Filters?.OnlyCached == true)
            {
                var cachedResponse = _cacheService.GetCache();
                if (cachedResponse != null)
                {
                    allRoutes.AddRange(_routeFilterService.FilterRoutes(cachedResponse.Routes, request.Filters));
                    
                    response = _routeStatisticsService.GetResponseWithStatistics(allRoutes);
                    return response;
                }
                return new SearchResponse { Routes = new List<Route>() };
            }
            var providerOneRoutes = await _providerOneService.SearchAsync(request, cancellationToken);
            var providerTwoRoutes = await _providerTwoService.SearchAsync(request, cancellationToken);

            allRoutes.AddRange(providerOneRoutes);
            allRoutes.AddRange(providerTwoRoutes);

            allRoutes = _routeFilterService.FilterRoutes(allRoutes, request.Filters);

            response = _routeStatisticsService.GetResponseWithStatistics(allRoutes);
            _cacheService.SetCache(response);
            return response;
        }
        public async Task<ProviderAvailabilityStatus> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var status = new ProviderAvailabilityStatus
            {
                ProviderOneAvailable = await _providerOneService.IsAvailableAsync(cancellationToken),
                ProviderTwoAvailable = await _providerTwoService.IsAvailableAsync(cancellationToken)
            };
            return status;
        }
    }
}