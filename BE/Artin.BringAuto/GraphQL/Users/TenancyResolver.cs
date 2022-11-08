using Artin.BringAuto.Shared.Tenants;
using Artin.BringAuto.Shared.Users;
using HotChocolate;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Orders
{
    public class TenancyResolver
    {
        public IQueryable<Tenant> GetTenants([Parent] User user, [Service] ITenantRepository tenantRepository)
        => tenantRepository.GetUserTenants(user.Id);

    }
}
