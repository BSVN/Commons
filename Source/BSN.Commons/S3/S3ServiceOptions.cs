namespace BSN.Commons.S3
{
    /// <summary>
    /// Used for S3 client configuration using .net core DI infrastructure.
    /// </summary>
    public class S3ServiceOptions
    {
        /// <summary>
        /// Provided access key.
        /// </summary>
        public string AccessKey { get; set; }
        
        /// <summary>
        /// S3 Server endpoint.
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Provided secret key.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Temporary token lifetime.
        /// </summary>
        /// <remarks>
        /// Only root user of the S3 Server is able to generate Temp tokens.
        /// </remarks>
        public int TempTokenLifeTime { get; set; } = int.MaxValue;
    }
}