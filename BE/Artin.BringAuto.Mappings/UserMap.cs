using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Shared.Users;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<Artin.BringAuto.DAL.Models.ApplicationUser, User>().ReverseMap();
            CreateMap<UpdateUser, ApplicationUser>();
            CreateMap<NewUser, ApplicationUser>()
                .IncludeBase<UpdateUser, ApplicationUser>();
                
        }
    }
}
