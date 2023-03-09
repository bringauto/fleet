using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Butons
{
    public interface IButtonRepository : IRepository<Button, NewButton, Button, int>
    {
    }
}
