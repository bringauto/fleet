using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Twillio;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Artin.BringAuto.Services
{
    public class TwillioCaller : ITwillioCaller
    {
        private readonly IOptions<TwillioConfig> options;
        private readonly ILogger<TwillioCaller> logger;

        public TwillioCaller(IOptions<TwillioConfig> options,
            ILogger<TwillioCaller> logger)
        {
            if (!String.IsNullOrWhiteSpace(options?.Value.SID) && !String.IsNullOrWhiteSpace(options.Value.Token))
                TwilioClient.Init(options.Value.SID, options.Value.Token);
            this.options = options;
            this.logger = logger;
        }
        public Task Call(string number, string messageUri)
        {
            if (String.IsNullOrWhiteSpace(options.Value.SID)
                || String.IsNullOrWhiteSpace(options.Value.Token))
                return Task.CompletedTask;

            if (!String.IsNullOrEmpty(number))
            {
                try
                {
                    var call = CallResource.Create(
                                twiml: messageUri,
                                from: new Twilio.Types.PhoneNumber(options.Value.FromNumber),
                                to: new Twilio.Types.PhoneNumber(number)
                                );

                    Console.WriteLine(call.Sid);
                }
                catch (Exception exc)
                {
                    logger.LogError(exc, "Cannot call by twilio");
                }
            }
            return Task.CompletedTask;
        }
    }
}
