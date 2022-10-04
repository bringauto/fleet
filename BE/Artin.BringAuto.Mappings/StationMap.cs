using Artin.BringAuto.Shared.Stations;
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
            CreateMap<Artin.BringAuto.DAL.Models.Station, Station>().ReverseMap();
            CreateMap<Artin.BringAuto.DAL.Models.Station, StationInfo>();
            CreateMap<NewStation, Artin.BringAuto.DAL.Models.Station>();
        }
    }
}
