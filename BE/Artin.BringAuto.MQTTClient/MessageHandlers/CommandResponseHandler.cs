using Artin.BringAuto.Shared.Orders;
using Google.Protobuf.ba_proto;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public class CommandResponseHandler : IHandler<CommandResponse>
    {
        private readonly IOrderRepository orderRepository;

        public CommandResponseHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task Handle(string company, string car, CommandResponse data)
        {
            await orderRepository.AcceptOrders(company, car);
        }
    }
}
