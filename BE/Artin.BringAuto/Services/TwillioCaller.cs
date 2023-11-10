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
            //queued - twilio received request to create call
            //initiated - number is dialed (NOT MENTIONED IN CallResource !!)
            //ringing - phone is ringing
            //in-progress - call picked up
            //completed - picked up call disconnected
            //busy - received busy response (phone already in call?)
            //no-answer - call not picked up for 60s
            //cancelled - call cancelled by rest api
            //failed - number unreachable
            var callStatus = CallResource.Fetch(sid).Status.ToString();

            while (callStatus != "in-progress" || 
                   callStatus != "completed" || 
                   callStatus != "busy" ||
                   callStatus != "no-answer" ||
                   callStatus != "cancelled" ||
                   callStatus != "failed")
            {
                Thread.Sleep(2000);
                callStatus = CallResource.Fetch(sid).Status.ToString();
            }

            return callStatus switch
            {
                "busy" => false,
                "no-answer" => false,
                _ => true,
            };
        }
    }
}
