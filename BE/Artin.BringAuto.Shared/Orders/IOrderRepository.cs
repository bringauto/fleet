using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Orders
{
    public interface IOrderRepository : IRepository<Order, NewOrder, UpdateOrder, int>
    {
        IQueryable<Order> ForCar(int carId);
        IQueryable<Order> ForStartStation(int id);

        Task<IList<Order>> GetRouteForCar(string company, string car);
        Task<OrderForCall> GetOrderForCall(int id);
        Task<bool> IsFirstOrder(int carId, int orderId);
        Task UpdateOrderStationStatus(Order order);
        Task AcceptOrders(string company, string car);
    }
}
