using Artin.BringAuto.Shared.Routes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class RouteMap : Profile
    {
        public RouteMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.Route, Route>()
               .ForMember(x => x.Id, x => x.MapFrom(s => s.Id))
               .ForMember(x => x.Name, x => x.MapFrom(s => s.Name))
               .ForMember(x => x.Color, x => x.MapFrom(s => s.Color))
               .ForMember(x => x.Stops, x => x.MapFrom(s => s.Stops))
               .ReverseMap();

            CreateMap<NewRoute, Artin.BringAuto.DAL.Models.Route>()
               .ForMember(x => x.Name, x => x.MapFrom(s => s.Name))
               .ForMember(x => x.Color, x => x.MapFrom(s => s.Color))
               .ForMember(x => x.Stops, x => x.MapFrom(s => s.Stops));

            CreateMap<Artin.BringAuto.DAL.Models.RouteStop, RouteStop>()
               .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
               .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude))
               .ForMember(x => x.Order, x => x.MapFrom(s => s.Order))
               .ForMember(x => x.Station, x => x.MapFrom(s => s.Station));

            CreateMap<RouteStop, Artin.BringAuto.DAL.Models.RouteStop>()
               .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
               .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude))
               .ForMember(x => x.Order, x => x.MapFrom(s => s.Order))
               .ForMember(x => x.StationId, x => x.MapFrom(s => s.Station.Id))
               .ForMember(x => x.Station, x => x.Ignore());


            CreateMap<RouteStopUpdate, Artin.BringAuto.DAL.Models.RouteStop>()
              .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
              .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude))
              .ForMember(x => x.Order, x => x.MapFrom(s => s.Order))
              .ForMember(x => x.StationId, x => x.MapFrom(s => s.StationId));

            CreateMap<RouteUpdate, Artin.BringAuto.DAL.Models.Route>()
              .ForMember(x => x.Id, x => x.MapFrom(s => s.Id))
              .ForMember(x => x.Name, x => x.MapFrom(s => s.Name))
              .ForMember(x => x.Color, x => x.MapFrom(s => s.Color))
              .ForMember(x => x.Stops, x => x.MapFrom(s => s.Stops));


        }
    }
}
