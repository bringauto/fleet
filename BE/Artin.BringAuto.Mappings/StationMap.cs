using Artin.BringAuto.Shared.Stops;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class StationMap : Profile
    {
        public StationMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.Stop, Stop>().ReverseMap();
            CreateMap<Artin.BringAuto.DAL.Models.Stop, StopInfo>();
            CreateMap<NewStop, Artin.BringAuto.DAL.Models.Stop>();
        }
    }
}
