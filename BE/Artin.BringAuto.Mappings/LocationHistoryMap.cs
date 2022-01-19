using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using AutoMapper;
using System;

namespace Artin.BringAuto.Mappings
{
    public class LocationHistoryMap : Profile
    {
        public LocationHistoryMap()
        {
            CreateMap<CarStatusInfo, NewLocationHistory>()
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude))
                .ForMember(x => x.CarId, x => x.Ignore());

            CreateMap<NewLocationHistory, DAL.Models.LocationHistory>()
                .ForMember(x => x.Time, x => x.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude))
                .ForMember(x => x.CarId, x => x.MapFrom(s => s.CarId));

            CreateMap<DAL.Models.LocationHistory, LocationHistory>()
                .ForMember(x => x.DateTime, x => x.MapFrom(s => s.Time))
                .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Longitude));

        }
    }
}
