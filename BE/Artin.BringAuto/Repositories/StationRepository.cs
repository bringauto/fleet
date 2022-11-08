using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Stops;
using AutoMapper;
using BringAuto.Server.Bases;

namespace BringAuto.Server.Repositories
{
    public class StationRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Stop, Stop, NewStop, Stop, int>, IStopRepository
    {
        public StationRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
