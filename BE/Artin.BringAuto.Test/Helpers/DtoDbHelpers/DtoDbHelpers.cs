using Movies.Api.Models;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Test.Helpers.DtoDbHelpers
{
    public static class DtoDbHelpers
    {
        public static Person DtoToDbPerson(PersonDto personDto)
        {
            Person person = new Person();
            personDto.Name = person.Name;
            personDto.Country = person.Country;
            personDto.Biography = person.Biography;
            personDto.Role = person.Role;
            personDto.Id = person.Id;
            personDto.BirthDate = person.BirthDate;

            return person;
        }

        public static PersonDto DbToDtoPerson(Person personDto)
        {
            PersonDto person = new PersonDto();
            personDto.Name = person.Name;
            personDto.Country = person.Country;
            personDto.Biography = person.Biography;
            personDto.Role = person.Role;
            personDto.Id = person.Id;
            personDto.BirthDate = person.BirthDate;

            return person;
        }
    }
}
