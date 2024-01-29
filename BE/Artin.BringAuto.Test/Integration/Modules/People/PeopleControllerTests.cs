using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Movies.Api.Controllers.Movies.Api.Controllers;
using Movies.Api.Interfaces;
using Movies.Api.Models;
using Movies.Api.Test.Helpers.DtoDbHelpers;
using Movies.Api.Test.Integration.Modules.People.Fixtures;
using Movies.Api.Test.Integration.Shared.Helpers;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Api.Test.Integration.Modules.People
{
    public class PeopleControllerTests
    {
        [Fact]
        public void GetPersonsTest()
        {
            // arrange
            var service = new Mock<IPersonService>();

            var persons = TestFixture.GetFakeDataPersons();
            List<PersonDto> personsDto = new List<PersonDto>();
            foreach(var person in persons)
            {
                PersonDto personDto = DtoDbHelpers.DbToDtoPerson(person);
                personsDto.Add(personDto);
            }
            service.Setup(x => x.GetAllPeople()).Returns(personsDto);

            var controller = new PeopleController(service.Object);

            // Act
            var results = controller.GetPeople();

            var count = results.Count();

            // Assert
            count.Equals(26);
        }

        [Fact]
        public void GetPerson()
        {
            // arrange
            var service = new Mock<IPersonService>();

            var persons = TestFixture.GetFakeDataPersons();
            var firstPerson = DtoDbHelpers.DbToDtoPerson(persons.First());
            service.Setup(x => x.GetPerson(1)).Returns(firstPerson);

            var controller = new PeopleController(service.Object);

            // act
            var result = controller.GetPerson(1);

            //assert
            result.Should().NotBeNull();
        }
    }
}
