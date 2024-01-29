using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Orders;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.Services
{
    public class IsInStationProcess : IIsInStationProcess
    {
        private readonly ITwillioCaller twillioCaller;
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<IsInStationProcess> logger;

        public IsInStationProcess(ITwillioCaller twillioCaller,
                                  IOrderRepository orderRepository,
                                  ILogger<IsInStationProcess> logger)
        {
            this.twillioCaller = twillioCaller;
            this.orderRepository = orderRepository;
            this.logger = logger;
        }
        public async Task ProcessStationArive(int orderId)
        {
            var order = await orderRepository.GetOrderForCall(orderId);
            if (order.CanCall)
            {
                logger.LogDebug($"Log order for call {JsonSerializer.Serialize(order)}");
                if (order?.ToStationId.HasValue == true && order?.ToStationStatus == Shared.Enums.OrderStopStatus.Done)
                {
                    logger.LogDebug("Call to ToStation");
                    await CallTo(order.ToStationPhone, GetTwiml(order.ToStationId));
                }
                else if (order?.FromStationId.HasValue == true && order?.FromStationStatus == Shared.Enums.OrderStopStatus.Done)
                {
                    logger.LogDebug("Call from ToStation");
                    await CallTo(order.FromStationPhone, GetTwiml(order.FromStationId));
                }
            }
            else
                logger.LogDebug("Call is forbiden (UnderTest)");
        }

        private string GetTwiml(int? id) => $"<Response><Play loop=\"10\">{GetMessageUri(id)}</Play></Response>";

        private string GetMessageUri(int? id) => "https://bringauto.com/wp-content/uploads/2021/10/BringAuto.mp3"; //TODO define per station

        private async Task CallTo(string phone, string twiml)
        {
            logger.LogDebug($"CallTo to `{phone ?? "not defined"}` with twiml {twiml}");
            if (!string.IsNullOrEmpty(phone))
                new Thread(async () =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    await twillioCaller.Call(phone, twiml);
                }).Start();
        }
    }
}
