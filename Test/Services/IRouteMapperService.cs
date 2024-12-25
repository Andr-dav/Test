using Test.Models;
using Route = Test.Models.Route;

namespace Test.Services
{
    public interface IRouteMapperService
    {
        List<Route> MapProviderOneSearchResponseToRoutes(ProviderOneSearchResponse providerResponse);
        List<Route> MapProviderTwoSearchResponseToRoutes(ProviderTwoSearchResponse providerResponse);
    }
}
