using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Stops;
using HotChocolate.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Stations
{
    public class StopMutation
    {
        private readonly IStopRepository stationRepository;

        public StopMutation(IStopRepository stationRepository)
        {
            this.stationRepository = stationRepository;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Stop> AddStop(NewStop station)
            => await stationRepository.AddAsync(station);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Stop> UpdateStop(Stop station)
            => await stationRepository.UpdateAsync(station);

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Stop> DeleteStop(int stationId)
            => await stationRepository.DeleteAsync(stationId);

    }
}
