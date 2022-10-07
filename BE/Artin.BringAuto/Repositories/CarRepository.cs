using Artin.BringAuto.Configuration;
using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Enums;
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
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BringAuto.Server.Repositories
{
    public class CarRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Car, Car, NewCar, UpdateCar, int>, ICarRepository
    {
        private readonly ILogger<CarRepository> logger;
        private readonly TokenManagement tokenManagement;

        public CarRepository(BringAutoDbContext dbContext,
                             IMapper mapper,
                             IOptions<TokenManagement> options,
                             ILogger<CarRepository> logger) : base(dbContext, mapper)
        {
            this.logger = logger;
            this.tokenManagement = options.Value;
        }

        public Task<string> GetSessionId(string companyName, string carName)
            => dbContext.Cars.Where(x => x.CompanyName == companyName && x.Name == carName)
                .Where(x => x.SessionLogged > DateTime.UtcNow.AddMinutes(-4))
                .Select(x => x.SessionId)
                .FirstOrDefaultAsync();

        public async Task SetSessionId(string companyName, string carName, string sessionId)
        {
            var car = await dbContext.Cars.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.CompanyName == companyName && x.Name == carName);
            if (car is object)
            {
                car.SessionId = sessionId;
                car.SessionLogged = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }
        }

        public Task<bool> IsKnownCar(string companyName, string carName)
            => dbContext.Cars.IgnoreQueryFilters().AnyAsync(x => EF.Functions.Collate(x.CompanyName, "SQL_Latin1_General_CP1_CS_AS") == companyName
            && EF.Functions.Collate(x.Name, "SQL_Latin1_General_CP1_CS_AS") == carName);

        public async Task NormalizeButtonAsync(int timeoutSec)
        {
            var expiredButtons = dbContext.ButtonStates.GroupBy(x => x.CarId).Select(x => new { x.Key, MAX = x.Max(b => b.DateTime) }).Where(x => x.MAX < DateTime.UtcNow.AddSeconds(-timeoutSec)).Select(x => x.Key);

            await dbContext.Cars.Where(x => expiredButtons.Contains(x.Id)).ForEachAsync(x => x.Button = ButtonStatus.NORMAL);

            await dbContext.SaveChangesAsync();
        }



        public async Task<Car> UpdateButonAsync(int carId, CarButtonStatus buttonStatus)
        {
            var car = await dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (buttonStatus.Status == ButtonStatus.PRESSED)
            {
                car.Button = buttonStatus.Status;
                await dbContext.SaveChangesAsync();
            }
            return await GetByIdAsync(carId);
        }

        public async Task<Car> UpdateStatusAsync(int carId, CarStatusInfo status)
        {
            var car = await dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            car = mapper.Map(status, car);
            await dbContext.SaveChangesAsync();
            return await GetByIdAsync(carId);
        }

        protected override async Task AfterAdd(Artin.BringAuto.DAL.Models.Car entity)
        {
            await base.AfterAdd(entity);
            entity.Token = new JwtSecurityTokenHandler().WriteToken(GenerateToken(CreateClaims(entity)));
        }

        protected override async Task BeforeUpdate(Artin.BringAuto.DAL.Models.Car entity)
        {
            await base.BeforeUpdate(entity);
            entity.Token = new JwtSecurityTokenHandler().WriteToken(GenerateToken(CreateClaims(entity)));
        }

        private IEnumerable<Claim> CreateClaims(Artin.BringAuto.DAL.Models.Car entity)
        {
            yield return new Claim(ClaimNames.CarId, entity.Id.ToString());
            yield return new Claim(ClaimNames.CarName, entity.Name.ToString());
        }

        private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            logger.LogInformation("Generate token");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                tokenManagement.Issuer,
                tokenManagement.Audience,
                claims,
                signingCredentials: credentials
            );
        }

        public async Task<bool> IsLoggedInAsync(string company, string car, string sessionId)
        {
            var carDal = await dbContext.Cars.FirstOrDefaultAsync(
                x => x.CompanyName == company
                && x.Name == car
                && x.SessionId == sessionId);

            if (carDal is object)
            {
                carDal.SessionLogged = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }
            return carDal is object;
        }

        public async Task<List<int>> GetCarStationOrder(string companyName, string carName)
        {
            return await dbContext.Cars.Where(
                x => x.CompanyName == companyName
                && x.Name == carName)
                .SelectMany(x => x.Route.Stops)
                .OrderBy(x => x.Order)
                .Where(x => x.StationId.HasValue)
                .Select(x => x.StationId.Value)
                .ToListAsync();

        }

        public async Task<string> GetCarRoute(string company, string car)
        {
            return await dbContext.Cars.Where(
                x => x.CompanyName == company
                && x.Name == car)
                .Select(x => x.Route.Name)
                .FirstOrDefaultAsync();
        }
    }
}
