using System;
using System.Net;

namespace BSN.Commons.Exceptions
{
    /// <summary>
    /// TODO: Complete Doc
    /// </summary>
    public class HttpRequestException : System.Net.Http.HttpRequestException
    {
        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        /// <param name="message"></param>
        public HttpRequestException(string message) : base(message) { }

        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HttpRequestException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// TODO: Complete Doc
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="statusCode"></param>
        public HttpRequestException(string message, Exception innerException, HttpStatusCode? statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
