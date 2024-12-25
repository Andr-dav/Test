using Test.Models;
using Route = Test.Models.Route;

namespace Test.Services
{
    public interface IRouteFilterService
    {
        List<Route> FilterRoutes(List<Route> routes, SearchFilters filters);
    }
}
