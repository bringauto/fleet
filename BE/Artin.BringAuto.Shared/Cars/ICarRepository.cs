using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Routes;
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

        Task SetSessionId(string companyName, string carName, string sessionId);
        Task<bool> IsKnownCar(string companyName, string carName);

        Task<string> GetSessionId(string companyName, string carName);
        Task<List<int>> GetCarStationOrder(string companyName, string carName);
        Task<bool> IsLoggedInAsync(string company, string car, string sessionId);
        Task<CarTrack> GetCarTrack(string company, string car);
    }
}
