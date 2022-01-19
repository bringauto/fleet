using Artin.BringAuto.DAL;
using Artin.BringAuto.Shared.Butons;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BringAuto.Server.Bases;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.Repositories
{
    public class ButtonRepository : RepositoryBase<Artin.BringAuto.DAL.Models.Button, Button, NewButton, Button, int>, IButtonRepository
    {
        public ButtonRepository(BringAutoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public IQueryable<Button> GetButtonsByCar(int carId)

            => dbContext.ButtonStates.Where(x => x.CarId == carId).ProjectTo<Button>(mapper.ConfigurationProvider);


        protected override async Task AfterAdd(DAL.Models.Button entity)
        {
            await base .AfterAdd(entity);
            dbContext.ButtonStates.RemoveRange(dbContext.ButtonStates.Where(x => x.CarId == entity.CarId && x.DateTime < DateTime.Now.AddDays(-7)));
        }
    }
}
