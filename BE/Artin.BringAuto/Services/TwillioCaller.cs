using Artin.BringAuto.Shared.Ifaces;
using Artin.BringAuto.Shared.Twillio;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                    for (int i = 0; i < 5; i++)
                    {
                        var call = CallResource.Create(
                                    twiml: messageUri,
                                    from: new Twilio.Types.PhoneNumber(options.Value.FromNumber),
                                    to: new Twilio.Types.PhoneNumber(number)
                                    );

                        Console.WriteLine(call.Sid);
                        
                        if (WaitForCallPickup(call.Sid))
                        {
                            break;
                        }
                    }
                }
                catch (Exception exc)
                {
                    logger.LogError(exc, "Cannot call by twilio");
                }
            }
            return Task.CompletedTask;
        }

        private static bool WaitForCallPickup(String sid)
        {
            var callInfo = CallResource.Fetch(sid);

            while (callInfo.Status == CallResource.StatusEnum.Ringing)
            {
                Thread.Sleep(2000);
                callInfo = CallResource.Fetch(sid);
            }

            return callInfo.Status.ToString() switch
            {
                "in-progress" => true,
                "cancelled" => true,
                "completed" => true,
                // busy(received busy signal)? no-answer(not picked up or hung up)? failed(number might not exist)?
                _ => false,
            };
        }
    }
}
