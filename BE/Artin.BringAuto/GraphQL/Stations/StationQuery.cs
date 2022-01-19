using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Stations;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Stations
{
    public class StationQuery
    {
        private readonly IStationRepository stationRepository;

        public StationQuery(IStationRepository stationRepository)
        {
            this.stationRepository = stationRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]

        public IQueryable<Station> GetStations()
        {
            return stationRepository.Load();
        }


    }
}
