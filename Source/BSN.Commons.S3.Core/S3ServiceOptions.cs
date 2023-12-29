namespace BSN.Commons.S3.Core
{
    /// <summary>
    /// S3 Service option for using on DI as configuration for S3Service client.
    /// </summary>
    public class S3ServiceOptions
    {
        /// <summary>
        /// S3 Service endpoint.
        /// </summary>
        /// <remarks>
        /// Endpoint should follow standard url:
        /// (e.g: http://localhost:8000)
        /// </remarks>
        public string EndPoint { get; set; }

        /// <summary>
        /// Provided access key.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Provided secret key.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Temporary token lifetime.
        /// </summary>
        /// <remark>
        /// Temporary token is being generated only by root user credential on S3 Service.
        /// </remark>
        public int TempTokenLifeTime { get; set; } = int.MaxValue;
    }
}