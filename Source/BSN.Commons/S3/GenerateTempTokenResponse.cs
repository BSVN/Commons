using System;

namespace BSN.Commons.S3
{
    /// <summary>
    /// Dedicated credential for specific object/bucket.
    /// </summary>
    public class GenerateTempTokenResponse
    {
        /// <summary>
        /// Credential model used for generating temporary tokens.
        /// </summary>
        /// <remarks>
        /// Temporary credential contains three main part: 
        ///     AccessKey
        ///     SecretKey
        ///     SessionToken
        /// </remarks>
        /// <param name="endPoint">S3 service endpoint.</param>
        /// <param name="accessKey">Provided access key.</param>
        /// <param name="secretKey">Provided secret key.</param>
        /// <param name="sessionToken">Session token.</param>
        /// <param name="notValidAfter">Indicates expiration time of temporary credential.</param>
        /// <param name="bucket">Issued bucket.</param>
        /// <param name="key">Issued key.</param>
        public GenerateTempTokenResponse(string endPoint,
                               string accessKey,
                               string secretKey,
                               string sessionToken,
                               DateTime notValidAfter,
                               string bucket,
                               string key = null)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SessionToken = sessionToken;
            NotValidAfter = notValidAfter;
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
        public DateTime NotValidAfter { get; set; }

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
        /// Provided session token.
        /// </summary>
        public string SessionToken { get; set; }
    }
}