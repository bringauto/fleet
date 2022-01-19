using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Maps;
using HotChocolate.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Maps
{
    public class MapMutation
    {
        private readonly IMapRepository mapRepository;

        public MapMutation(IMapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> AddMap(NewMap map)
        {
            return await mapRepository.AddAsync(map);
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> UpdateMap(Map map)
        {
            return await mapRepository.UpdateAsync(map);
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> DeleteMap(int mapId)
        {
            return await mapRepository.DeleteAsync(mapId);
        }
    }
}
