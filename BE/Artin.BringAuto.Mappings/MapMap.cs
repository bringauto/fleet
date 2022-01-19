using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Maps;
using AutoMapper;
using System.Text;

namespace Artin.BringAuto.Mappings
{
    public class MapMap : Profile
    {
        public MapMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.Map, Map>()
                .ReverseMap();
            CreateMap<NewMap, Artin.BringAuto.DAL.Models.Map>();
        }
    }
}
