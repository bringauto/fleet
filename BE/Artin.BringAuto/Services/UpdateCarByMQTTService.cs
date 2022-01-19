using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.MQTTClient;
using Artin.BringAuto.Shared.Cars;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.Services
{
    public class UpdateCarByMQTTService : IUpdateCarByMQTTService
    {
        private readonly BringAutoDbContext dbContext;
        
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        public UpdateCarByMQTTService(BringAutoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateCarAsync(string carHwId, string topic, double value)
        {
            await semaphoreSlim.WaitAsync();

            var car = await dbContext.Cars.FirstOrDefaultAsync(x => x.HwId == carHwId);
            if (car is object)
            {
                UpdateCar(car, topic, value);
            }
            try
            {
                await dbContext.SaveChangesAsync();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private void UpdateCar(DAL.Models.Car car, string topic, double value)
        {
            switch (topic)
            {
                case "fuel": car.Fuel = value; break;
                case "speed": car.Speed = value; break;
                case "position/latitude": car.Latitude = value; break;
                case "position/longitude": car.Longitude = value; break;
                default:
                    break;
            }
        }

        public async Task UpdateCarPositionAsync(string companyName, string carName, double latitude, double longitude, double fuel)
        {
            var car = await dbContext.Cars.FirstOrDefaultAsync(x => x.CompanyName == companyName && x.Name == carName);
            if (car is object)
            {
                car.Latitude = latitude;
                car.Longitude = longitude;
                car.Fuel = fuel;

                var latHistory =  await dbContext.LocationHistory.OrderByDescending(x => x.Time).FirstOrDefaultAsync(x=>x.CarId == car.Id); 

                var newHistory = new LocationHistory();
                newHistory.CarId = car.Id;
                newHistory.Latitude = latitude;
                newHistory.Longitude = longitude;
                newHistory.Time = DateTime.UtcNow;
                dbContext.LocationHistory.Add(newHistory);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
