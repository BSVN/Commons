using System;

namespace BSN.Commons.S3.Abstraction
{
    /// <summary>
    /// Dedicated credential model for specific object.
    /// </summary>
    public class CredentialModel
    {
        public CredentialModel(string endPoint,
                               string accessKey,
                               string secretKey,
                               string sessionToken,
                               DateTime expiration,
                               string bucket,
                               string key = null)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SessionToken = sessionToken;
            Expiration = expiration;
            Bucket = bucket;
            Key = key;
            EndPoint = endPoint;
        }
        
        /// <summary>
        /// Issued bucket.
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// S3 service endpoint.
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Credential expiration time.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Indicates whether this credential is generated for an object or not.
        /// <remarks>
        /// Credential will be generated for buckets or objects.
        /// </remarks>
        /// </summary>
        public bool IsObjectPriviledge => !string.IsNullOrEmpty(Key);

        /// <summary>
        /// Issued object (key).
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Provided AccessKey.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Provided secret key.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Provided temporary token.
        /// </summary>
        public string SessionToken { get; set; }
    }
}