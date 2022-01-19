using Artin.BringAuto.Shared.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public interface IRouteRepository : IRepository<Route, NewRoute, RouteUpdate, int>
    {
        
    }
}
