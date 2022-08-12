using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Stops;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Stations
{
    public class StopQuery
    {
        private readonly IStopRepository stopRepository;

        public StopQuery(IStopRepository stopRepository)
        {
            this.stopRepository = stopRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]

        public IQueryable<Stop> GetStop()
        {
            return stopRepository.Load();
        }


    }
}
