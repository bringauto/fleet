using Artin.BringAuto.Shared.Butons;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class ButtonMap : Profile
    {
        public ButtonMap()
        {
            CreateMap<DAL.Models.Button, Shared.Butons.Button>().ReverseMap();

            CreateMap<Shared.Butons.NewButton, DAL.Models.Button>()
                .ForMember(x => x.DateTime, x => x.MapFrom(s => DateTime.UtcNow));

            CreateMap<CarButtonStatus, NewButton>();
        }

    }
}
