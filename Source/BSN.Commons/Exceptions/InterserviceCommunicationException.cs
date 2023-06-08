using System;

namespace BSN.Commons.Exceptions
{
    public class InterserviceCommunicationException : Exception
    {
        public InterserviceCommunicationException() : base() { }

        public InterserviceCommunicationException(string message) : base(message) { }

        public InterserviceCommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
