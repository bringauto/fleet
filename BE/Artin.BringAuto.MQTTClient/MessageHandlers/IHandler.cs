using Google.Protobuf.ba_proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public interface IHandler<T>
    {
        Task Handle(string company, string car, T data);
    }
}
