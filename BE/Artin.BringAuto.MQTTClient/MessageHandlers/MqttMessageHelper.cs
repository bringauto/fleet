using Google.Protobuf;
using Google.Protobuf.ba_proto;
using Microsoft.Extensions.Logging;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public static class MqttMessageHelper
    {
        public static MqttApplicationMessage CreateMqttMessage(this MessageIndustrialPortal messageIndustrialPortal, string company, string car, ILogger logger)
        {
            var msg = new MqttApplicationMessage()
            {
                Topic = $"{company}/default/{car}/industrial_portal"
            };
            using (var ms = new MemoryStream())
            {
                using (var stream = new CodedOutputStream(ms))
                {
                    messageIndustrialPortal.WriteTo(stream);
                    stream.Flush();
                    msg.Payload = ms.ToArray();
                }
            }

            logger.LogDebug($"Message to send {messageIndustrialPortal}");
            
            return msg;
        }
    }
}
