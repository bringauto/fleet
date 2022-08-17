using Artin.BringAuto.GraphQL.Orders;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Stops;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Stops
{
    public class StationExtension : ObjectTypeExtension<Stop>
    {
        protected override void Configure(IObjectTypeDescriptor<Stop> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field<OrderResolver>(x => x.GetStationFromOrders(default, default))
                .Name("ordersFromThisStop")
                .UsePaging<ObjectType<Order>>()
                .UseFiltering()
                .UseSorting();

        }
    }
}
