using Artin.BringAuto.Shared.Butons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Cars
{
    public interface ICarRepository : IRepository<Car, NewCar, UpdateCar, int>
    {

        Task<Car> UpdateStatusAsync(int carId, CarStatusInfo status);
        Task<Car> UpdateButonAsync(int carId, CarButtonStatus buttonStatus);
        Task NormalizeButtonAsync(int timeoutSec);

        Task SetSessionId(string companyName, string carName, string sessionId);
        Task<bool> IsKnownCar(string companyName, string carName);

        Task<string> GetSessionId(string companyName, string carName);
        Task<List<int>> GetCarStationOrder(string companyName, string carName);
        Task<bool> IsLoggedInAsync(string company, string car, string sessionId);
        Task<string> GetCarRoute(string company, string car);
    }
}
