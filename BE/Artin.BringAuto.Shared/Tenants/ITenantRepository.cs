using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Tenants
{
    public interface ITenantRepository : IRepository<Tenant, NewTenant, Tenant, int>
    {
        IQueryable<Tenant> GetUserTenants(string userId);
    }
}
