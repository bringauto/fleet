using System.Linq;

namespace Artin.BringAuto.Shared.LocationHistory
{
    public interface ILocationHistoryRepository : IRepository<LocationHistory, NewLocationHistory, UpdateLocationHistory, int>
    {
        IQueryable<LocationHistory> FetchByCar(int carId);

    }
}
