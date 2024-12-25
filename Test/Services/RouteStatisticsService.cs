using Test.Models;
using Route = Test.Models.Route;

namespace Test.Services
{
    public class RouteStatisticsService : IRouteStatisticsService
    {
        public SearchResponse GetResponseWithStatistics(List<Route> routes)
        {
            if (routes == null || !routes.Any())
            {
                return new SearchResponse
                {
                    Routes = new List<Route>(),
                    MinPrice = 0,
                    MaxPrice = 0,
                    MinMinutesRoute = 0,
                    MaxMinutesRoute = 0
                };
            }
            var minPrice = routes.Min(route => route.Price);
            var maxPrice = routes.Max(route => route.Price);

            var minMinutes = routes.Min(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes);
            var maxMinutes = routes.Max(r => (int)(r.DestinationDateTime - r.OriginDateTime).TotalMinutes);

            return new SearchResponse
            {
                Routes = routes,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinMinutesRoute = minMinutes,
                MaxMinutesRoute = maxMinutes
            };
        }
    }
}