using Test.Models;
using Route = Test.Models.Route;

namespace Test.Services
{
    public interface IRouteStatisticsService
    {
        SearchResponse GetResponseWithStatistics(List<Route> routes);
    }
}
