using Artin.BringAuto.GraphQL.Orders;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Stations;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Stations
{
    public class StationExtension : ObjectTypeExtension<Station>
    {
        protected override void Configure(IObjectTypeDescriptor<Station> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field<OrderResolver>(x => x.GetStationFromOrders(default, default))
                .Name("ordersFromThisStation")
                .UsePaging<ObjectType<Order>>()
                .UseFiltering()
                .UseSorting();

        }
    }
}
