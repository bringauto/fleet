using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Orders;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class OrderMap : Profile
    {
        public OrderMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.Order, Order>()
                .ForMember(x => x.From, x => x.MapFrom(s => s.FromStation))
                .ForMember(x => x.To, x => x.MapFrom(s => s.ToStation))
                .ForMember(x => x.FromStationPhone, x => x.MapFrom(s => s.FromStationPhone))
                .ForMember(x => x.ToStationPhone, x => x.MapFrom(s => s.ToStationPhone))
                .ReverseMap();
            CreateMap<NewOrder, Artin.BringAuto.DAL.Models.Order>()
                .ForMember(x => x.FromStationPhone, x => x.MapFrom(s => s.FromStationPhone))
                .ForMember(x => x.ToStationPhone, x => x.MapFrom(s => s.ToStationPhone))
                .ForMember(x => x.Status, x => x.MapFrom(s => Artin.BringAuto.Shared.Enums.OrderStatus.ToAccept));

            CreateMap<UpdateOrder, Artin.BringAuto.DAL.Models.Order>()
                .ForMember(x => x.FromStationPhone, x => x.MapFrom(s => s.FromStationPhone))
                .ForMember(x => x.ToStationPhone, x => x.MapFrom(s => s.ToStationPhone));

            CreateMap<Artin.BringAuto.DAL.Models.Order, OrderForCall>()
                .ForMember(x => x.Id, x => x.MapFrom(s => s.Id))
                .ForMember(x => x.CanCall, x => x.MapFrom(s => !s.Car.UnderTest))
                .ForMember(x => x.FromStationId, x => x.MapFrom(s => s.FromStation.Id))
                .ForMember(x => x.ToStationId, x => x.MapFrom(s => s.ToStation.Id))
                .ForMember(x => x.FromStationStatus, x => x.MapFrom(s => s.FromStationStatus))
                .ForMember(x => x.ToStationStatus, x => x.MapFrom(s => s.ToStationStatus))
                .ForMember(x => x.FromStationPhone, x => x.MapFrom(s => s.FromStationPhone ?? s.FromStation.ContactPhone))
                .ForMember(x => x.ToStationPhone, x => x.MapFrom(s => s.ToStationPhone ?? s.ToStation.ContactPhone));


        }
    }
}
