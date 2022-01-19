using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Orders
{
    public class OrderQuery
    {
        private readonly IOrderRepository orderRepository;

        public OrderQuery(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public IQueryable<Order> GetOrders()
        {
            return orderRepository.Load();
        }


    }
}
