using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Test.Integration.Shared.Helpers
{
    public class RequestHelper
    {
        public static StringContent MakeApiRequest(object content)
        {
            var request = new StringContent(JsonConvert.SerializeObject(content));
            request.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");
            return request;
        }
    }
}
