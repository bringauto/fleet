using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared
{
    public interface ICanAccessOrderProvider: ICurrentUserId
    {

        bool CanAccessAllOrders { get; }
        

    }
}
