using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Ifaces
{
    public interface IIsInStationProcess
    {
        public Task ProcessStationArive(int orderId);
    }
}
