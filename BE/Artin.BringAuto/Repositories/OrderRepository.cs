using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Enums;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Orders;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BringAuto.Server.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BringAuto.Server.Repositories
{
    public class OrderRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Order, Order, NewOrder, UpdateOrder, int>, IOrderRepository
    {
        private readonly IAllowedPriority allowedPriority;
        private readonly ICurrentUserId currentUserId;
        private readonly ICurrentRoles currentRoles;

        public OrderRepository(BringAutoDbContext dbContext, IMapper mapper,
            IAllowedPriority allowedPriority,
            ICurrentUserId currentUserId,
            ICurrentRoles currentRoles) : base(dbContext, mapper)
        {
            this.allowedPriority = allowedPriority;
            this.currentUserId = currentUserId;
            this.currentRoles = currentRoles;
        }

        public IQueryable<Order> ForCar(int carId)
        => dbContext.Orders.Where(x => x.CarId == carId).ProjectTo<Order>(mapper.ConfigurationProvider);

        public IQueryable<Order> ForStartStation(int startStationId)
        => dbContext.Orders.Where(x => x.FromStationId == startStationId).ProjectTo<Order>(mapper.ConfigurationProvider);



        protected override async Task BeforeAdd(Artin.BringAuto.DAL.Models.Order entity)
        {
            await CheckCurrentCarPermitions(entity);

            entity.Priority = allowedPriority.CheckOrderPriority(entity.Priority);
            entity.UserId = currentUserId.CurrentUserId;

            if (await dbContext.Stops.AnyAsync(x => x.Id == entity.ToStationId && !x.Deleted)
                && (!entity.FromStationId.HasValue || await dbContext.Stops.AnyAsync(x => x.Id == entity.FromStationId && !x.Deleted))
               )
                await base.BeforeAdd(entity);
            else
                throw new ArgumentException("Cannot use deleted or unknown stations");
        }

        private async Task CheckCurrentCarPermitions(Artin.BringAuto.DAL.Models.Order entity)
        {
            if (!currentRoles.Roles.Contains(RoleNames.Admin))
            {
                var underTest = (await dbContext.Cars.FirstOrDefaultAsync(x => x.Id == entity.CarId))?.UnderTest == true;
                if (underTest)
                    throw new ArgumentException("Car is under test. Only admin can manage orders");
            }

        }

        protected override async Task BeforeUpdate(Artin.BringAuto.DAL.Models.Order entity)
        {
            await CheckCurrentCarPermitions(entity);
            entity.Priority = allowedPriority.CheckOrderPriority(entity.Priority);

            if (await dbContext.Stops.AnyAsync(x => x.Id == entity.ToStationId && !x.Deleted)
                && (!entity.FromStationId.HasValue || await dbContext.Stops.AnyAsync(x => x.Id == entity.FromStationId && !x.Deleted))
               )
                await base.BeforeUpdate(entity);
            else
                throw new ArgumentException("Cannot use deleted or unknown stations");
        }

        public async Task<IList<Order>> GetRouteForCar(string company, string car)
        {
            return await dbContext.Orders.Where(x => x.Car.Name == car && x.Car.CompanyName == company)
                .Where(x =>
                x.Status == Artin.BringAuto.Shared.Enums.OrderStatus.ToAccept
                || x.Status == Artin.BringAuto.Shared.Enums.OrderStatus.Accepted
                || x.Status == Artin.BringAuto.Shared.Enums.OrderStatus.InProgress)
                .IgnoreQueryFilters()
                .ProjectTo<Order>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.FromStationStatus)
                .ThenByDescending(x => x.Priority)
                .ThenByDescending(x => x.Arrive)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public Task<OrderForCall> GetOrderForCall(int id)
            => dbContext.Orders.IgnoreQueryFilters().ProjectTo<OrderForCall>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateOrderStationStatus(Order order)
        {
            var dalOrder = await dbContext.Orders.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == order.Id);

            if (dalOrder.FromStation is null)
                dalOrder.FromStationStatus = Artin.BringAuto.Shared.Enums.OrderStopStatus.Done;
            else
                dalOrder.FromStationStatus = order.FromStationStatus;
            dalOrder.ToStationStatus = order.ToStationStatus;

            dalOrder.Status = dalOrder.FromStationStatus == dalOrder.ToStationStatus
                && dalOrder.ToStationStatus == Artin.BringAuto.Shared.Enums.OrderStopStatus.Done
                ? Artin.BringAuto.Shared.Enums.OrderStatus.Done : Artin.BringAuto.Shared.Enums.OrderStatus.InProgress;

            await dbContext.SaveChangesAsync();
        }

        public async Task AcceptOrders(string company, string car)
        {
            await dbContext.Orders.Where(x => x.Car.Name == car && x.Car.CompanyName == company)
                .Where(x => x.Status == OrderStatus.ToAccept)
                .IgnoreQueryFilters()
                .ForEachAsync(x => x.Status = OrderStatus.Accepted);

            var orders = await dbContext.Orders.Where(x => x.Car.Name == car && x.Car.CompanyName == company)
                .Where(x =>
                 x.Status == OrderStatus.Accepted
                || x.Status == OrderStatus.InProgress)
                .IgnoreQueryFilters()
                .OrderByDescending(x => x.FromStationStatus)
                .ThenByDescending(x => x.Priority)
                .ThenByDescending(x => x.Arrive)
                .ThenBy(x => x.Id)
                .ToListAsync();

            var status = OrderStatus.InProgress;
            foreach (var order in orders)
            {
                order.Status = status;
                if (status == OrderStatus.Accepted)
                {
                    order.FromStationStatus = OrderStopStatus.InQueue;
                    order.ToStationStatus = OrderStopStatus.InQueue;
                }
                status = OrderStatus.Accepted;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsFirstOrder(int carId, int orderId)
        {
            return (await dbContext.Orders.CountAsync(x => x.CarId == carId
                  && x.Status != Artin.BringAuto.Shared.Enums.OrderStatus.Done
                  && x.Status != Artin.BringAuto.Shared.Enums.OrderStatus.Canceled
                  && x.Id != orderId)) == 0;
        }
    }
}
