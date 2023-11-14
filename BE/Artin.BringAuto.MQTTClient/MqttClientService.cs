using Artin.BringAuto.MQTTClient.MessageHandlers;
using Artin.BringAuto.Shared.Cars;
using Google.Protobuf.ba_proto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private Timer timer;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        private readonly HandlerSelector handlerSelector;
        private readonly ILogger<MqttClientService> logger;
        private IMqttClientOptions options;
        private readonly IServiceProvider serviceProvider;

        public MqttClientService(IMqttClientOptions options,
            IServiceProvider serviceProvider,
            IMqttClient mqttClient,
            HandlerSelector handlerSelector,
            ILogger<MqttClientService> logger
            )
        {
            this.options = options;
            this.serviceProvider = serviceProvider;
           
            this.mqttClient = mqttClient;
            this.handlerSelector = handlerSelector;
            this.logger = logger;
            ConfigureMqttClient();

        }

        private async void SendNotification(object state)
        {
            if (await semaphoreSlim.WaitAsync(10))
            {
                try
                {
                    using var myScope = serviceProvider.CreateScope();
                    {
                        var mqttClient = myScope.ServiceProvider.GetService<IMqttClient>();                        
                        if (!mqttClient.IsConnected)
                            await mqttClient.ReconnectAsync();
                        await myScope.ServiceProvider.GetService<ISendCarRouteService>().SendToAllCars();
                    }
                }
                catch (Exception exc)
                {
                    logger.LogError(exc, "cannot comunicate with MQTT broker");
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
        }

        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }

        static Regex topicRegex = new Regex("vehicle/(?<id>[^/]*)/telemetry/(?<topic>.*)$", RegexOptions.Compiled);
        static Regex carDaemonRegex = new Regex("(?<companyName>[^/]*)/default/(?<carName>.*)/daemon$", RegexOptions.Compiled);
        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var updateCarByMQTTService = scope.ServiceProvider.GetRequiredService<IUpdateCarByMQTTService>();
                var match = topicRegex.Match(eventArgs.ApplicationMessage.Topic);
                if (match.Success)
                {
                    var value = Double.Parse(Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture);
                    await updateCarByMQTTService.UpdateCarAsync(match.Groups["id"].Value, match.Groups["topic"].Value, value);
                }

                match = carDaemonRegex.Match(eventArgs.ApplicationMessage.Topic);
                if (match.Success)
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    var message = MessageDaemon.Parser.ParseFrom(eventArgs.ApplicationMessage.Payload);
                    logger.LogDebug($"Received message for car {match.Groups["companyName"].Value}/{match.Groups["carName"].Value}");
                    logger.LogDebug($"Data:{message}");
                    await handlerSelector.ProcessMessage(scope, match.Groups["companyName"].Value, match.Groups["carName"].Value, message);
                    watch.Stop();
                    logger.LogInformation($"Message at topic {eventArgs.ApplicationMessage.Topic}---{message.TypeCase} was processed in {watch.ElapsedMilliseconds} ms ");
                }
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Cannot parse message");
            }
        }



        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");
            await mqttClient.SubscribeAsync("#");
        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await mqttClient.ConnectAsync(options);
                if (!mqttClient.IsConnected)
                {
                    await mqttClient.ReconnectAsync();
                }
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Cannot connect MQTT broker");
            }
            timer = new Timer(SendNotification, null, new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 5));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await timer.DisposeAsync();
            if (cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDiconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }
    }
}
