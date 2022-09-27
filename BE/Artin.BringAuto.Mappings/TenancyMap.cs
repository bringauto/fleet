using Artin.BringAuto.Shared.Tenants;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Mappings
{
    public class TenancyMap : Profile
    {
        public TenancyMap()
        {
            CreateMap<DAL.Models.UserTenancy, Tenant>()
                .ForMember(x => x.Id, x => x.MapFrom(s => s.TenantId))
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Tenant.Name));
        }
    }
}
