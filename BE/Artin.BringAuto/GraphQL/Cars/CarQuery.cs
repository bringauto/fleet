using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Cars;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Cars
{
    public class CarQuery
    {
        private readonly ICarRepository carRepository;

        public CarQuery(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseSelection]
        [Authorize(Roles = new[] { RoleNames.Admin, RoleNames.Driver, RoleNames.Privileged, RoleNames.User })]
        public IQueryable<Car> GetCars()
        {
            return carRepository.Load();
        }


    }
}
