using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.LocationHistory;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BringAuto.Server.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.Repositories
{
    public class CarLocationHistoryRepository : RepositoryBase<DAL.Models.LocationHistory, LocationHistory, NewLocationHistory, UpdateLocationHistory, int>, ILocationHistoryRepository
    {
        public CarLocationHistoryRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public IQueryable<LocationHistory> FetchByCar(int carId)
            => dbContext.LocationHistory.Where(x => x.Car.Id == carId)
            .ProjectTo<LocationHistory>(mapper.ConfigurationProvider);

        protected override async Task AfterAdd(DAL.Models.LocationHistory entity)
        {
            await base.AfterAdd(entity);

            await dbContext.Database.ExecuteSqlRawAsync($"DELETE from {nameof(DAL.Models.LocationHistory)} where {nameof(DAL.Models.LocationHistory)} < DATEADD(hh,-5,sysdatetime()) ");
        }
    }
}
