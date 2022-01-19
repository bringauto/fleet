using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public interface IUpdateCarByMQTTService
    {
        public Task UpdateCarAsync(string carHwId, string topic, double value);

        public Task UpdateCarPositionAsync(string companyName, string carName, double latitude, double longitude, double fuel);
    }
}
