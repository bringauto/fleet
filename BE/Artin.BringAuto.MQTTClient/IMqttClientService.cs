using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public interface IMqttClientService : IHostedService,
                                            IMqttClientConnectedHandler,
                                            IMqttClientDisconnectedHandler,
                                            IMqttApplicationMessageReceivedHandler
    {
    }
}
