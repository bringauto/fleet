using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Routes
{
    public class CarTrack
    {
        public string Name { get; set; }

        public IEnumerable<CarTrackStop> RouteStops { get; set; }
    }
}
