using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Routes;
using HotChocolate.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Routes
{
    public class RouteMutation
    {
        private readonly IRouteRepository routeRepository;

        public RouteMutation(IRouteRepository routeRepository)
        {
            this.routeRepository = routeRepository;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Route> AddRoute(NewRoute route)
            => await routeRepository.AddAsync(route);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Route> UpdateRoute(RouteUpdate route)
            => await routeRepository.UpdateAsync(route);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Route> DeleteRoute(int routeId)
            => await routeRepository.DeleteAsync(routeId);

    }
}
