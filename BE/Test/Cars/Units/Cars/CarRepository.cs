using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Artin.BringAuto.Test.Cars.Units.Cars
{
    public class CarRepository
    {
        [Theory]
        [InlineData("octavia", "octavia")]
        public static void IsKnownCar_CarName(string carNameDb, string carName)
        {
            //toto se delat nesmi, test spadne protoze lze testovat jen na realne databazi!
            //https://stackoverflow.com/questions/70693625/the-collate-method-is-not-supported-because-the-query-has-switched-to-client-e
            var efName = EF.Functions.Collate(carNameDb, "SQL_Latin1_General_CP1_CS_AS");
            //efName.Should().NotBeNullOrWhiteSpace();
            carName.Should().Be(efName);
        }
    }
}
