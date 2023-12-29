using System.Net;

namespace BSN.Commons.S3.Abstraction
{
    /// <summary>
    /// Amazon s3 response model.
    /// </summary>
    public class S3Response
    {
        /// <summary>
        /// Response status code.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Provided message.
        /// </summary>
        public string Message { get; set; }
    }
}