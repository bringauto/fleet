using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.LocationHistory;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICarRepository carRepository;
        private readonly ILocationHistoryRepository locationHistoryRepository;
        private readonly IButtonRepository buttonRepository;
        private readonly IMapper mapper;

        public CarController(ICarRepository carRepository, ILocationHistoryRepository locationHistoryRepository, 
            IButtonRepository buttonRepository,
            IMapper mapper)
        {
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
    }
}
