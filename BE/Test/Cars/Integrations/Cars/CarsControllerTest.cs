using Artin.BringAuto.DAL;
using Artin.BringAuto.DAL.Models;
using Artin.BringAuto.GraphQL.Cars;
using Artin.BringAuto.Shared.Cars;
using AutoMapper;
using BringAuto.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Test.Cars.Integrations.Cars
{
    public class CarsControllerTests
    {
        [Fact]
        public void GetCarsTest()
        {
            // arrange

            var items = TestFixture.GetFakeDataCars();
            IMapper mapper = null;

            var service = new CarRepository(null, null, null, null);

            var controller = new CarQuery(service);

            // Act
            var results = controller.GetCars();

            var count = results.Count();

            // Assert
            count.Equals(26);
        }
    }
}
