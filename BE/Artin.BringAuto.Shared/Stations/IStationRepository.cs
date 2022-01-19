using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Stations
{
    public interface IStationRepository: IRepository<Station, NewStation, Station, int>
    {
        
    }
}
