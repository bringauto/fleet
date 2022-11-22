using Artin.BringAuto.MQTTClient.MessageHandlers;
using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Orders;
using Google.Protobuf.ba_proto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public class SendCarRouteService : ISendCarRouteService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICarRepository carRepository;
        private readonly IMqttClient mqttClient;
        private readonly ILogger<SendCarRouteService> logger;

        public SendCarRouteService(IOrderRepository orderRepository,
            ICarRepository carRepository,
            IMqttClient mqttClient,
            ILogger<SendCarRouteService> logger)
        {
            this.orderRepository = orderRepository;
            this.carRepository = carRepository;
            this.mqttClient = mqttClient;
            this.logger = logger;
        }
        public async Task SendCarRoute(string company, string car)
        {
            if (!String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(car))
            {
                var route = await orderRepository.GetRouteForCar(company, car);
                var sessionId = await carRepository.GetSessionId(company, car);

                if (!String.IsNullOrEmpty(sessionId))
                {
                    var msg = new MessageIndustrialPortal
                    {
                        Command = new Command()
                        {
                            SessionId = sessionId,
                            CarCommand = new CarCommand
                            {
                                Action = CarCommand.Types.Action.Start
                            }
                        }
                    };

                    foreach (var order in route)
                    {
                        if (order.From is object && order.FromStationStatus != Shared.Enums.OrderStopStatus.Done)
                            msg.Command.CarCommand.Stops.Add(new Stop() { To = order.From.Name });
                        if (order.ToStationStatus != Shared.Enums.OrderStopStatus.Done)
                            msg.Command.CarCommand.Stops.Add(new Stop() { To = order.To.Name });
                    }
                    var carTrack = await carRepository.GetCarTrack(company, car);
                    if (carTrack is not null)
                        msg.Command.CarCommand.Route = carTrack.Name ?? string.Empty;
                    if (carTrack?.RouteStops is not null)
                        msg.Command.CarCommand.RouteStations.Add(
                            carTrack.RouteStops.Select(s => new Station()
                            {
                                Name = s.Name,
                                Position = new Station.Types.Position()
                                {
                                    Latitude = s.Latitude,
                                    Longitude = s.Longitude
                                }
                            }));
                    await mqttClient.PublishAsync(msg.CreateMqttMessage(company, car, logger));
                }
            }
        }

        public async Task SendToAllCars()
        {
            var cars = await carRepository.Load().ToListAsync();
            foreach (var car in cars)
            {
                await SendCarRoute(car.CompanyName, car.Name);
            }
        }
    }
}
