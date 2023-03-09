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
    }
}
