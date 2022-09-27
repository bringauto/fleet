using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Routes;
using Artin.BringAuto.Shared.Tenants;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BringAuto.Server.Bases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BringAuto.Server.Repositories
{
    public class TenantRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Tenant, Tenant, NewTenant, Tenant, int>, ITenantRepository
    {

        public TenantRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public IQueryable<Tenant> GetUserTenants(string userId)
            => dbContext.Set<Artin.BringAuto.DAL.Models.UserTenancy>().Where(x => x.UserId == userId)
                .ProjectTo<Tenant>(mapper.ConfigurationProvider);
    }
}
