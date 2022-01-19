using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Maps;
using Artin.BringAuto.Shared.Orders;
using Artin.BringAuto.Shared.Routes;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BringAuto.Server.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BringAuto.Server.Repositories
{
    public class RouteRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Route, Route, NewRoute, RouteUpdate, int>, IRouteRepository
    {

        public RouteRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        protected override IQueryable<Artin.BringAuto.DAL.Models.Route> AddIncludesForUpdate(IQueryable<Artin.BringAuto.DAL.Models.Route> source)
            => source.Include(x => x.Stops);

        protected override void BeforeUpdateMap(Artin.BringAuto.DAL.Models.Route dal)
        {
            dal?.Stops?.Clear();
            base.BeforeUpdateMap(dal);
        }

        protected override void BeforeDelete(int id)
        {
            dbContext.RouteStops.RemoveRange(dbContext.RouteStops.Where(x => x.Route.Id == id));
        }
    }
}
