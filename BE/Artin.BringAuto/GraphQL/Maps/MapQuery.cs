using Artin.BringAuto.Helpers;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Maps;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Maps
{
    public class MapQuery
    {
        private readonly IMapRepository mapRepository;

        public MapQuery(IMapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public IQueryable<Map> GetMaps()
        {
            return mapRepository.Load();
        }


        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public async Task<MapPosition> GetMapPosition(int mapId, Double latitude, Double longitude)
        {
            var map = await mapRepository.GetByIdAsync(mapId);
            if (latitude < map.MinLatitude || latitude > map.MaxLatitude
                ||
                longitude < map.MinLongitude || longitude > map.MaxLongitude)
                return null; //Its out of map.

            return new MapPosition()
            {
                LatitudeRelative = RelativeMapHelper.RelativePosition(latitude, map.MinLatitude, map.MaxLatitude, map.Height),
                LongitudeRelative = RelativeMapHelper.RelativePosition(longitude, map.MinLongitude, map.MaxLongitude, map.Width),
            };
        }

       


    }
}
