using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [Serializable]
    class BlackDuckIntegrationException : Exception
    {
        public BlackDuckIntegrationException(): base()
        {
        }

        public BlackDuckIntegrationException(string message) : base(message)
        {
        }

        public BlackDuckIntegrationException(HttpResponseMessage content) : base($"Response returned with: {content.ReasonPhrase}")
        {
        }

        public BlackDuckIntegrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BlackDuckIntegrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
