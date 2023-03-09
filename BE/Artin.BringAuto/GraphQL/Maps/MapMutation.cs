using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Maps;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Maps
{
    [ApiController]
    [Route("maps")]
    public class MapMutation
    {
        private readonly IMapRepository mapRepository;

        public MapMutation(IMapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        [HttpPost("create")]
        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> AddMap(NewMap map)
        {
            return await mapRepository.AddAsync(map);
        }

        [HttpPost("update")]
        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> UpdateMap(Map map)
        {
            return await mapRepository.UpdateAsync(map);
        }

        [HttpPost("delete")]
        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Map> DeleteMap(int mapId)
        {
            return await mapRepository.DeleteAsync(mapId);
        }
    }
}
