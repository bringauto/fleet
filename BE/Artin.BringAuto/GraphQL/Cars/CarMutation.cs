﻿using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Cars;
using HotChocolate.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Artin.BringAuto.GraphQL.Cars
{
    public class CarMutation
    {
        private readonly ICarRepository carRepository;

        public CarMutation(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Car> AddCar(NewCar car)
        {
            return await carRepository.AddAsync(car);
        }

        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver })]
        public async Task<Car> UpdateCar(UpdateCar car)
        {
            return await carRepository.UpdateAsync(car);
        }



        [Authorize(Roles = new[] { RoleNames.Admin })]
        public async Task<Car> DeleteCar(int carId)
        {
            return await carRepository.DeleteAsync(carId);
        }
    }
}
