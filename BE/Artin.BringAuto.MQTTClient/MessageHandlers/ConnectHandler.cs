using Artin.BringAuto.Shared.Cars;
using Google.Protobuf;
using Google.Protobuf.ba_proto;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient.MessageHandlers
{
    public class ConnectHandler : IHandler<Connect>
    {
        private readonly IMqttClient mqttClient;
        private readonly ICarRepository carRepository;
        private readonly ILogger<ConnectHandler> logger;

        public ConnectHandler(IMqttClient mqttClient,
            ICarRepository carRepository,
            ILogger<ConnectHandler> logger)
        {
            this.mqttClient = mqttClient;
            this.carRepository = carRepository;
            this.logger = logger;
        }
        public async Task Handle(string company, string car, Connect data)
        {

            var response = new MessageIndustrialPortal();
            response.ConnectReponse = new ConnectResponse();
            if (await carRepository.IsKnownCar(company, car))
            {
                response.ConnectReponse.SessionId = data.SessionId;
                response.ConnectReponse.Type = ConnectResponse.Types.Type.Ok;
                await carRepository.SetSessionId(company, car, data.SessionId);
            }
            else
            {
                response.ConnectReponse.SessionId = data.SessionId;
                response.ConnectReponse.ErrorMessage = "Unknown pair Car and CompanyName";
                response.ConnectReponse.Type = ConnectResponse.Types.Type.ConnectionRefused;
                logger.LogWarning(response.ConnectReponse.ErrorMessage);

            }
            await mqttClient.PublishAsync(response.CreateMqttMessage(company, car, logger));
        }
    }
}
