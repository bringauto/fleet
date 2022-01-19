using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.MQTTClient
{
    public class MqttConfig
    {
        public bool Enable { get; set; } = false;
        public String Host { get; set; }
        public int Port { get; set; }
        public List<CertFile> CertFiles { get; set; }

        public bool ValidateCertificate { get; set; }
        public bool UseTls { get; set; }
    }

    public class CertFile
    {
        public String Cert { get; set; }
        public String PrivateKey { get; set; }
    }
}
