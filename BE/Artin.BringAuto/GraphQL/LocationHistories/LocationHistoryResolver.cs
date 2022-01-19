using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using HotChocolate;
using System.Linq;

namespace Artin.BringAuto.GraphQL.LocationHistories
{
    public class LocationHistoryResolver
    {
        public IQueryable<LocationHistory> GetCarLocationHistory([Parent] Car car, [Service] ILocationHistoryRepository locationHistoryRepository)
       => locationHistoryRepository.FetchByCar(car.Id);
    }
}
