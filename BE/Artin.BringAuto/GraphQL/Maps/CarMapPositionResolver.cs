using Artin.BringAuto.Helpers;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Maps;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Maps
{
    public class CarMapPositionResolver
    {
        public async Task<MapPosition> GetMapPositionAsync([Service] IMapRepository mapRepository, [Parent]Car car, int mapId)
        {
            var map = await mapRepository.GetByIdAsync(mapId);
            if (map is null 
                || 
                car.Latitude < map.MinLatitude || car.Latitude> map.MaxLatitude
                ||
                car.Longitude < map.MinLongitude || car.Longitude > map.MaxLongitude)
                return null; //Its out of map.

            return new MapPosition()
            {
                LatitudeRelative = RelativeMapHelper.RelativePosition(car.Latitude, map.MinLatitude, map.MaxLatitude, map.Height),
                LongitudeRelative = RelativeMapHelper.RelativePosition(car.Longitude, map.MinLongitude, map.MaxLongitude, map.Width),
            };
        }
    }
}
