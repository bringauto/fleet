using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.Test.Cars.Integrations.Fixtures;
using BringAuto.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Test.Cars.Integrations.Cars
{
    public class CarControllerTest : IClassFixture<CarFixture>
    {
        private readonly CarFixture _fixture;
        private readonly DbSet<Car> _entities;
        private const string _modelRoute = "api/v1/cars";

        public CarControllerTest(CarFixture fixture)
        {
            _fixture = fixture;
            _entities = _fixture.DbContext.Set<Car>();
        }

        [Fact]
        public void IsKnownCar_Not_Undertest_Test()
        {

        }
    }
}
