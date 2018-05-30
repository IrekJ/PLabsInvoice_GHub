using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace PLabsInvoice.MessageHandlers
{
    public class APIKeyMessageHandler :DelegatingHandler 
    {
        private const string APIKeyToCheck = "AFADF546AGFDAF4AS4F6SDAF68AF684A6F6A4F4AF";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false;
            IEnumerable<string> requstHeaders;
            bool checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requstHeaders);
            if (checkApiKeyExists)
            {
                if (requstHeaders.FirstOrDefault().Equals(APIKeyToCheck))
                {
                    validKey = true;
                }
            }
            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
            }
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}