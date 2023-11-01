using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.ba_proto;

namespace Artin.BringAuto.MQTTClient
{
    public interface ISendCarRouteService
    {
        public Task SendCarRoute(string company, string car);

        public Task SendToAllCars();

        public Task<CarCommand.Types.Action> DetermineCarAction(string company, string car);
    }
}
