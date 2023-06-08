using System;
using System.Net;

namespace BSN.Commons.Exceptions
{
    public class HttpRequestException : System.Net.Http.HttpRequestException
    {
        public HttpStatusCode? StatusCode { get; }

        public HttpRequestException(string message) : base(message) { }

        public HttpRequestException(string message, Exception innerException) : base(message, innerException) { }

        public HttpRequestException(string message, Exception innerException, HttpStatusCode? statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
