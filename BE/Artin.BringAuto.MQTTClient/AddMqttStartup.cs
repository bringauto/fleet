using Artin.BringAuto.MQTTClient.MessageHandlers;
using Google.Protobuf.ba_proto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public static class AddMqttStartup
    {
        public static void AddMqtt(this IServiceCollection services, IConfiguration configuration)
        {
            var cfg = configuration.GetSection(nameof(MqttConfig));
            services.Configure<MqttConfig>(cfg);

            services.AddSingleton<IMqttClientOptions>(x =>
            {
                var config = x.GetRequiredService<IOptions<MqttConfig>>().Value;
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(config.Host, config.Port)
                    .WithCleanSession();
                options = options.WithTls(new MqttClientOptionsBuilderTlsParameters()
                {
                    UseTls = config.UseTls,
                    IgnoreCertificateRevocationErrors = !config.ValidateCertificate,
                    IgnoreCertificateChainErrors = !config.ValidateCertificate,
                    AllowUntrustedCertificates = !config.ValidateCertificate,
                    SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                    Certificates = GetCertificates(config.CertFiles)

                });
                return options.Build();
            });

            services.AddSingleton<IMqttClient>(s => new MqttFactory().CreateMqttClient());
            services.AddScoped<ISendCarRouteService, SendCarRouteService>();


            services.AddSingleton<HandlerSelector>();
            services.AddTransient<IHandler<Connect>, ConnectHandler>();
            services.AddTransient<IHandler<Status>, StatusHandler>();
            services.AddTransient<IHandler<CommandResponse>, CommandResponseHandler>();

            if (cfg.Get<MqttConfig>().Enable)
                services.AddHostedService<MqttClientService>();
        }

        private static IEnumerable<X509Certificate> GetCertificates(List<CertFile> certFiles)
        {
            foreach (var certFile in certFiles)
            {
                if (String.IsNullOrEmpty(certFile.PrivateKey))
                    yield return new X509Certificate2(certFile.Cert);
                else
                {
                    var cert = X509Certificate2.CreateFromPemFile(certFile.Cert, certFile.PrivateKey);
                    yield return new X509Certificate(cert.Export(X509ContentType.Pfx, "asdf"), "asdf"); //Workaround
                }
            }

        }
    }
}
