using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Stations;
using Artin.BringAuto.Shared.Tenants;
using Artin.BringAuto.Shared.Users;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Orders
{
    public class TenancyResolver
    {
        public IQueryable<Tenant> GetTenants([Parent] User user, [Service] ITenantRepository tenantRepository)
        => tenantRepository.GetUserTenants(user.Id);

    }
}
