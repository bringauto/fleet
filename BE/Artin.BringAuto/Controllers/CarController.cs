using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Enums;
using Artin.BringAuto.Shared.LocationHistory;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> logger;
        private readonly ICarRepository carRepository;
        private readonly ILocationHistoryRepository locationHistoryRepository;
        private readonly IButtonRepository buttonRepository;
        private readonly IMapper mapper;

        public CarController(ICarRepository carRepository, ILocationHistoryRepository locationHistoryRepository, 
            IButtonRepository buttonRepository,
            IMapper mapper,
            ILogger<CarController> logger)
        {
            this.logger = logger;
            this.carRepository = carRepository;
            this.locationHistoryRepository = locationHistoryRepository;
            this.buttonRepository = buttonRepository;
            this.mapper = mapper;
        }
        [HttpPost("status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = PolicyNames.IsCar)]
        public async Task<ActionResult<Car>> UpdateCarStatus(CarStatusInfo carStatusInfo)
        {
            if (Int32.TryParse(HttpContext.User.FindFirst(ClaimNames.CarId).Value, out var carId))
            {
                var carChanges = carRepository.UpdateStatusAsync(carId, carStatusInfo);
                var newHistory = mapper.Map<NewLocationHistory>(carStatusInfo);
                newHistory.CarId = carId;
                var result = await carChanges;
                await locationHistoryRepository.AddAsync(newHistory);
                return result;
            }
            return null;
        }

        [HttpPost("button")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = PolicyNames.IsCar)]
        public async Task<ActionResult<Car>> UpdateCarButtonStatus(CarButtonStatus buttonStatus)
        {
            if (Int32.TryParse(HttpContext.User.FindFirst(ClaimNames.CarId).Value, out var carId))
            {
                var carChanges = carRepository.UpdateButonAsync(carId, buttonStatus);
                var newHistory = mapper.Map<NewButton>(buttonStatus);
                newHistory.CarId = carId;
                var result = await carChanges;
                await buttonRepository.AddAsync(newHistory);
                return result;
            }
            return null;
        }

        [HttpPost("startstop")]
        [AllowAnonymous]
        public async Task<ActionResult<Car>> StartStopCar(string companyName, string carName, CarStatusInfo aaaa)
        {
            logger.LogInformation($"startstop request reached company: {companyName}, car: {carName}");
            //var carStatus = await carRepository.GetCarStatus(companyName, carName);
            //var startstop = await carRepository.GetStartStopToggle(companyName, carName);
            /*TODO backend has no idea about car state
            if (carStatus == CarStatus.Driving || startstop)
            {*/
                aaaa.Longitude = 0;
                aaaa.Latitude = 0;
                aaaa.Status = CarStatus.Driving;

                await carRepository.UpdateStatusAsync(3, aaaa);
                //await carRepository.UpdateStartStopToggle(companyName, carName);
                logger.LogInformation("startstop request finished");
                return null;
            //}
            //TODO zisti id auta a skus to s tym zmenit, ked nie zisti jak sa autorizovat
        }
    }
}
