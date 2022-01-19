using Artin.BringAuto.GraphQL.Buttons;
using Artin.BringAuto.GraphQL.LocationHistories;
using Artin.BringAuto.GraphQL.Maps;
using Artin.BringAuto.GraphQL.Orders;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using Artin.BringAuto.Shared.Orders;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Cars
{
    public class CarExtension : ObjectTypeExtension<Car>
    {
        protected override void Configure(IObjectTypeDescriptor<Car> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field<OrderResolver>(x => x.GetCarOrders(default, default))
                .Name("orders")
                .UsePaging<ObjectType<Order>>()
                .UseFiltering()
                .UseSorting();

            descriptor.Field<LocationHistoryResolver>(x => x.GetCarLocationHistory(default, default))
               .Name("locationHistory")
               .UsePaging<ObjectType<LocationHistory>>()
               .UseFiltering()
               .UseSorting();

            descriptor.Field<ButtonHistoryResolver>(x => x.GetCarButtonHistory(default, default))
               .Name("buttonHistory")
               .UsePaging<ObjectType<Button>>()
               .UseFiltering()
               .UseSorting();

            descriptor.Field<CarMapPositionResolver>(x => x.GetMapPositionAsync(default, default, default))
                .Name("mapPosition")
                .Argument("mapId", x =>
                {
                    x.Type<NonNullType<IntType>>();
                });

        }
    }
}
