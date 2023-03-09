using Artin.BringAuto.MQTTClient;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Orders
{
    [ApiController]
    [Route("orders")]
    public class OrderMutation
    {
        private readonly IOrderRepository orderRepository;
        private readonly ISendCarRouteService sendCarRouteService;
        private readonly ICarRepository carRepository;
        private readonly ITwillioCaller twillioCaller;

        public OrderMutation(IOrderRepository orderRepository,
            ISendCarRouteService sendCarRouteService,
            ICarRepository carRepository,
            ITwillioCaller twillioCaller
            )
        {
            this.orderRepository = orderRepository;
            this.sendCarRouteService = sendCarRouteService;
            this.carRepository = carRepository;
            this.twillioCaller = twillioCaller;
        }

        [HttpPost("create")]
        [Authorize(Roles = new[] { RoleNames.Privileged, RoleNames.User, RoleNames.Admin, RoleNames.Driver })]
        public async Task<Order> AddOrder(NewOrder order)
        {
            try
            {
                var result = await orderRepository.AddAsync(order);
                await CheckIfFirstOrderAsync(order.CarId, result.Id);
                await sendCarRouteService.SendCarRoute(result.Car.CompanyName, result.Car.Name);
                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private async Task CheckIfFirstOrderAsync(int carId, int orderId)
        {
            var car = await carRepository.Load().FirstOrDefaultAsync(x => !x.UnderTest && x.Id == carId && x.CarAdminPhone != null && x.CallTwiml != null);
            if (car is Car && await orderRepository.IsFirstOrder(carId, orderId))
            {
                if (car?.CarAdminPhone != null && car?.CallTwiml != null)
                    await twillioCaller.Call(car.CarAdminPhone, car.CallTwiml);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = new[] { RoleNames.Driver, RoleNames.Privileged, RoleNames.User, RoleNames.Admin })]
        public async Task<Order> UpdateOrder(UpdateOrder order)
        {
            var result = await orderRepository.UpdateAsync(order);
            await sendCarRouteService.SendCarRoute(result.Car.CompanyName, result.Car.Name);
            return result;
        }

        [HttpDelete("delete")]
        [Authorize(Roles = new[] { RoleNames.Privileged, RoleNames.User, RoleNames.Driver, RoleNames.Admin })]
        public async Task<Order> DeleteOrder(int orderId)
        {
            var result = await orderRepository.DeleteAsync(orderId);
            await sendCarRouteService.SendCarRoute(result.Car.CompanyName, result.Car.Name);
            return result;
        }
    }
}
