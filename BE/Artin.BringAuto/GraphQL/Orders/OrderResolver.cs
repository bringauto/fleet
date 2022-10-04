using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Stations;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Orders
{
    public class OrderResolver
    {
        public IQueryable<Order> GetCarOrders([Parent] Car car, [Service] IOrderRepository orderRepository)
        => orderRepository.ForCar(car.Id);

        public IQueryable<Order> GetStationFromOrders([Parent] Station station, [Service] IOrderRepository orderRepository)
       => orderRepository.ForStartStation(station.Id);
    }
}
