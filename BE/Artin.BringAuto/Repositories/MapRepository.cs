using Artin.BringAuto.Configuration;
using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Maps;
using AutoMapper;
using BringAuto.Server.Bases;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BringAuto.Server.Repositories
{
    public class MapRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Map, Map, NewMap, Map, int>, IMapRepository
    {

        public MapRepository(BringAutoDbContext dbContext,
                             IMapper mapper) : base(dbContext, mapper)
        {
        }

    }
}
