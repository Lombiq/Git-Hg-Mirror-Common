using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace GitHgMirror.Common.Routes
{
    public class MirroringConfigurationRoutes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes()) routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
            {
                new RouteDescriptor
                {
                    Route = new Route(
                        "MirroringConfiguration",
                        new RouteValueDictionary
                        {
                            {"area", "GitHgMirror.Common"},
                            {"controller", "MirroringConfiguration"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "GitHgMirror.Common"}
                        },
                        new MvcRouteHandler()),
                    Priority = 99
                }
            };
        }
    }
}