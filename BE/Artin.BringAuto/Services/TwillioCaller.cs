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
        public async Task<Task> Call(string number, string messageUri)
        {
            if (String.IsNullOrWhiteSpace(options.Value.SID)
                || String.IsNullOrWhiteSpace(options.Value.Token))
                return Task.CompletedTask;

            if (!String.IsNullOrEmpty(number))
            {
                try
                {
                    // Try to call 5 times or until call is picked up
                    for (int i = 0; i < options.Value.CallRetryCount; i++)
                    {
                        var call = CallResource.Create(
                                    twiml: messageUri,
                                    from: new Twilio.Types.PhoneNumber(options.Value.FromNumber),
                                    to: new Twilio.Types.PhoneNumber(number)
                                    );

                        Console.WriteLine(call.Sid);
                        
                        if (await WaitForCallPickup(call.Sid))
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

        /*
            Queries call state every 2 seconds for 2 minutes until it is picked up or fails

            queued - twilio received request to create call
            initiated - number is dialed (NOT MENTIONED IN CallResource doc)
            ringing - phone is ringing
            in-progress - call picked up
            completed - picked up call disconnected
            busy - received busy response (phone already in call?)
            no-answer - call not picked up for 60s
            cancelled - call cancelled by rest api
            failed - number unreachable
        */
        private async Task<bool> WaitForCallPickup(String sid)
        {
            var callStatus = (await CallResource.FetchAsync(sid)).Status.ToString();
            int timeoutCount = 0;

            while (callStatus != "in-progress" &&
                   callStatus != "completed" &&
                   callStatus != "busy" &&
                   callStatus != "no-answer" &&
                   callStatus != "cancelled" &&
                   callStatus != "failed")
            {
                Task.Delay(options.Value.CallStatusQueryIntervalMS).Wait();
                callStatus = (await CallResource.FetchAsync(sid)).Status.ToString();
                timeoutCount++;

                // Endpoint in case twilio behaves differently
                if (timeoutCount >= options.Value.CallStatusQueryTimeoutCount)
                    return true;
            }

            return callStatus switch
            {
                "no-answer" => false,
                _ => true,
            };
        }
    }
}
