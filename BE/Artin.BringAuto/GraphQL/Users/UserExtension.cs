using Artin.BringAuto.GraphQL.Buttons;
using Artin.BringAuto.GraphQL.LocationHistories;
using Artin.BringAuto.GraphQL.Maps;
using Artin.BringAuto.GraphQL.Orders;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Tenants;
using Artin.BringAuto.Shared.Users;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Cars
{
    public class UserExtension : ObjectTypeExtension<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field<TenancyResolver>(x => x.GetTenants(default, default))
                .Name("tenants")
                .UsePaging<ObjectType<Tenant>>()
                .UseFiltering()
                .UseSorting();



        }
    }
}
