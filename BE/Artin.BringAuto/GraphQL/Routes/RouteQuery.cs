using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Routes;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Routes
{
    public class RouteQuery
    {
        private readonly IRouteRepository routeRepository;

        public RouteQuery(IRouteRepository routeRepository)
        {
            this.routeRepository = routeRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]

        public IQueryable<Route> GetRoutes()
        {
            return routeRepository.Load();
        }
    }
}
