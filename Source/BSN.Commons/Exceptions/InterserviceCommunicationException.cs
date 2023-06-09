using System;

namespace BSN.Commons.Exceptions
{
    /// <summary>
    /// TODO: Complete Doc
    /// </summary>
    public class InterserviceCommunicationException : Exception
    {
        /// <summary>
        /// TODO: Complete Doc
        /// </summary>        
        public InterserviceCommunicationException() : base() { }

        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        /// <param name="message"></param>
        public InterserviceCommunicationException(string message) : base(message) { }

        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InterserviceCommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
