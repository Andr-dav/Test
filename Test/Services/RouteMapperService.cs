using System.Collections.Generic;
using System.Linq;
using Test.Models;
using TestTask;
using Route = Test.Models.Route;

namespace Test.Services
{
    public class RouteMapperService : IRouteMapperService
    {

        public List<Route> MapProviderOneSearchResponseToRoutes(ProviderOneSearchResponse providerResponse)
        {
            if (providerResponse?.Routes == null)
            {
                return new List<Route>();
            }

            return providerResponse.Routes.Select(r => new Route
            {
                Id = Guid.NewGuid(),
                Origin = r.From,
                Destination = r.To,
                OriginDateTime = r.DateFrom,
                DestinationDateTime = r.DateTo,
                Price = r.Price,
                TimeLimit = r.TimeLimit
            }).ToList();
        }

        public List<Route> MapProviderTwoSearchResponseToRoutes(ProviderTwoSearchResponse providerResponse)
        {
            if (providerResponse?.Routes == null)
            {
                return new List<Route>(); 
            }
            return providerResponse.Routes.Select(r => new Route
            {
                Id = Guid.NewGuid(),
                Origin = r.Departure.Point,
                Destination = r.Arrival.Point,
                OriginDateTime = r.Departure.Date,
                DestinationDateTime = r.Arrival.Date,
                Price = r.Price,
                TimeLimit = r.TimeLimit
            }).ToList();
        }
    }
}