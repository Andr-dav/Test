using Test.Models;
using Route = Test.Models.Route;

namespace Test.Services
{
    public class RouteFilterService : IRouteFilterService
    {
        public List<Route> FilterRoutes(List<Route> routes, SearchFilters filters)
        {
            if (filters == null)
            {
                return routes;
            }

            if (filters.DestinationDateTime.HasValue)
            {
                routes = routes.Where(r => r.DestinationDateTime <= filters.DestinationDateTime.Value).ToList();
            }
            if (filters.MaxPrice.HasValue)
            {
                routes = routes.Where(r => r.Price <= filters.MaxPrice.Value).ToList();
            }
            if (filters.MinTimeLimit.HasValue)
            {
                routes = routes.Where(r => r.TimeLimit >= filters.MinTimeLimit.Value).ToList();
            }
            return routes;
        }
    }
}