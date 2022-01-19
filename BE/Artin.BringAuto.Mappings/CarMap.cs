using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Enums;
using AutoMapper;
using System;

namespace Artin.BringAuto.Mappings
{
    public class CarMap : Profile
    {
        public CarMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.Car, Car>().ReverseMap();
            CreateMap<NewCar, Artin.BringAuto.DAL.Models.Car>();
            CreateMap<UpdateCar, Artin.BringAuto.DAL.Models.Car>();
            CreateMap<CarStatusInfo, Artin.BringAuto.DAL.Models.Car>()
                .ForMember(x => x.Status, x => x.MapFrom(MapCarStatus));

        }

        private CarStatus MapCarStatus(CarStatusInfo arg, Artin.BringAuto.DAL.Models.Car destination, CarStatus status)
            => arg.Status ?? destination.Status;
    }
}
