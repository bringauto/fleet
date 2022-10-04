using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Stations;
using AutoMapper;
using BringAuto.Server.Bases;

namespace BringAuto.Server.Repositories
{
    public class StationRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Station, Station, NewStation, Station, int>, IStationRepository
    {
        public StationRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
