using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Orders;
using Google.Protobuf.ba_proto;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public class StatusHandler : IHandler<Status>
    {
        private readonly IMqttClient mqttClient;
        private readonly IOrderRepository orderRepository;
        private readonly ICarRepository carRepository;
        private readonly ISendCarRouteService sendCarRouteService;
        private readonly IUpdateCarByMQTTService updateCarByMQTTService;
        private readonly IIsInStationProcess isInStationProcess;
        private readonly ILogger<StatusHandler> logger;

        public StatusHandler(IMqttClient mqttClient, IOrderRepository orderRepository,
            ICarRepository carRepository,
             ISendCarRouteService sendCarRouteService,
             IUpdateCarByMQTTService updateCarByMQTTService,
             IIsInStationProcess isInStationProcess,
             ILogger<StatusHandler> logger)
        {
            this.mqttClient = mqttClient;
            this.orderRepository = orderRepository;
            this.carRepository = carRepository;
            this.sendCarRouteService = sendCarRouteService;
            this.updateCarByMQTTService = updateCarByMQTTService;
            this.isInStationProcess = isInStationProcess;
            this.logger = logger;
        }
        public async Task Handle(string company, string car, Status data)
        {
            if (await carRepository.IsLoggedInAsync(company, car, data.SessionId))
            {
                var route = await orderRepository.GetRouteForCar(company, car);

                if (data.Server.Type == Status.Types.ServerError.Types.Type.ServerError)
                {
                    await MarkHistoryRoute(data, route);
                }

                if (data.CarStatus.State == CarStatus.Types.State.InStop)
                {
                    await UpdateCurrentStation(data, route);
                }
                await updateCarByMQTTService.UpdateCarPositionAsync(company, car, data.CarStatus.Telemetry.Position.Latitude, data.CarStatus.Telemetry.Position.Longitude, data.CarStatus.Telemetry.Fuel);

                MessageIndustrialPortal msg = new MessageIndustrialPortal();
                msg.StatusResponse = new StatusResponse() { SessionId = data.SessionId, Type = StatusResponse.Types.Type.Ok };
                await mqttClient.PublishAsync(msg.CreateMqttMessage(company, car, logger));

                if (data.Server.Type == Status.Types.ServerError.Types.Type.ServerError
                    || data.CarStatus.State == CarStatus.Types.State.InStop)
                    await sendCarRouteService.SendCarRoute(company, car);
            }
        }

        private async Task UpdateCurrentStation(Status data, IList<Order> route)
        {
            var searchedStop = route.FirstOrDefault();
            if (searchedStop is object)
            {
                if (searchedStop?.From?.Name == data.CarStatus.Stop.To)
                {
                    if (searchedStop.FromStationStatus != Shared.Enums.OrderStopStatus.Done)
                    {
                        searchedStop.FromStationStatus = Shared.Enums.OrderStopStatus.Done;
                        await orderRepository.UpdateOrderStationStatus(searchedStop);
                        await isInStationProcess.ProcessStationArive(searchedStop.Id);
                    }
                }
                else if ((searchedStop.FromStationStatus == Shared.Enums.OrderStopStatus.Done
                    || searchedStop.From is null)
                    && searchedStop.To.Name == data.CarStatus.Stop.To)
                {
                    if (searchedStop.ToStationStatus != Shared.Enums.OrderStopStatus.Done)
                    {
                        searchedStop.ToStationStatus = Shared.Enums.OrderStopStatus.Done;
                        await orderRepository.UpdateOrderStationStatus(searchedStop);
                        await isInStationProcess.ProcessStationArive(searchedStop.Id);
                    }
                }
            }
        }

        private async Task MarkHistoryRoute(Status data, IList<Order> route)
        {
            foreach (var stop in data.Server.Stops)
            {
                var searchedStop = route.FirstOrDefault(x => (x.From?.Name == stop.To && x.FromStationStatus != Shared.Enums.OrderStopStatus.Done)
                || ((x.FromStationStatus == Shared.Enums.OrderStopStatus.Done || x.From is null) && x.To.Name == stop.To && x.ToStationStatus != Shared.Enums.OrderStopStatus.Done));
                if (searchedStop is not null)
                {
                    if (searchedStop?.From?.Name == stop.To)
                        searchedStop.FromStationStatus = Shared.Enums.OrderStopStatus.Done;
                    if (searchedStop?.To.Name == stop.To)
                        searchedStop.ToStationStatus = Shared.Enums.OrderStopStatus.Done;

                    if (searchedStop.ToStationStatus == Shared.Enums.OrderStopStatus.Done
                        && searchedStop.FromStationStatus == Shared.Enums.OrderStopStatus.Done)
                        searchedStop.Status = Shared.Enums.OrderStatus.Done;

                    await orderRepository.UpdateOrderStationStatus(searchedStop);

                }
            }
        }
    }
}
