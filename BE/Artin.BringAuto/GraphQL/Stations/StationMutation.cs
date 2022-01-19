using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Stations;
using HotChocolate.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Stations
{
    public class StationMutation
    {
        private readonly IStationRepository stationRepository;

        public StationMutation(IStationRepository stationRepository)
        {
            this.stationRepository = stationRepository;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Station> AddStation(NewStation station)
            => await stationRepository.AddAsync(station);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Station> UpdateStation(Station station)
            => await stationRepository.UpdateAsync(station);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Station> DeleteStation(int stationId)
            => await stationRepository.DeleteAsync(stationId);

    }
}
