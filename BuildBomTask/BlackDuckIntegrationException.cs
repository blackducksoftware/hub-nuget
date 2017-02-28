using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace com.blackducksoftware.integration.hub.nuget
{
    class BlackDuckIntegrationException : Exception
    {

        public BlackDuckIntegrationException(): base()
        {

        }

        public BlackDuckIntegrationException(HttpResponseMessage content) : base($"Response returned with: {content.ReasonPhrase}")
        {
       
        }
    }
}
