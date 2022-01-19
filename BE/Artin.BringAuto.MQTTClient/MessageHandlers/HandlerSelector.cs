using Google.Protobuf.ba_proto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public class HandlerSelector
    {
        private readonly IServiceProvider serviceProvider;
        private struct HandlerRef
        {
            public Type HandlerType { get; set; }
            public MethodInfo Method { get; set; }

            public PropertyInfo PropertyInfo { get; set; }

        }
        readonly Dictionary<MessageDaemon.TypeOneofCase, HandlerRef> mapDictionary = new Dictionary<MessageDaemon.TypeOneofCase, HandlerRef>();

        public HandlerSelector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            mapDictionary.Add(MessageDaemon.TypeOneofCase.Connect, GetHandlerRef<Connect>());
            mapDictionary.Add(MessageDaemon.TypeOneofCase.Status, GetHandlerRef<Status>());
            mapDictionary.Add(MessageDaemon.TypeOneofCase.CommandResponse, GetHandlerRef<CommandResponse>());

        }

        private HandlerRef GetHandlerRef<T>()
        {
            HandlerRef result = new();
            result.HandlerType = typeof(IHandler<T>);
            result.Method = result.HandlerType.GetMethod(nameof(IHandler<T>.Handle));
            result.PropertyInfo = typeof(MessageDaemon).GetProperty(typeof(T).Name);
            return result;
        }




        public Task ProcessMessage(IServiceScope scope, string companyName, string carName, MessageDaemon message)
        {
            if (mapDictionary.TryGetValue(message.TypeCase, out var type))
            {
                var handler = scope.ServiceProvider.GetService(type.HandlerType);
                return type.Method.Invoke(handler, new object[] { companyName, carName, type.PropertyInfo.GetValue(message) }) as Task;
            }
            return Task.CompletedTask;
        }
    }
}
