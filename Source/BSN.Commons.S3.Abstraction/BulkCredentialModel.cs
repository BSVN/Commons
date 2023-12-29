using System;
using System.Collections.Generic;

namespace BSN.Commons.S3.Abstraction
{
    [Obsolete("[R.Noei] I'm not sure if we need this anymore.")]
    public class BulkCredentialModel
    {
        public BulkCredentialModel(string endPoint,
                                   string accessKey,
                                   string secretKey,
                                   string sessionToken,
                                   DateTime expiration,
                                   string bucket,
                                   List<string> kies = null)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SessionToken = sessionToken;
            Expiration = expiration;
            Bucket = bucket;
            Kies = kies;
            EndPoint = endPoint;
        }

        /// <summary>
        /// کد کاربری
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// آدرس باکتی که مجوز برای آن صادر شده است
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// آدرس سرویس دهنده آمازون اس 3
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// انقضای توکن
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// نام شی ای که مجوز برای آن صادر شده است
        /// </summary>
        public List<string> Kies { get; set; }

        /// <summary>
        /// رمز کاربری
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// توکن
        /// </summary>
        public string SessionToken { get; set; }
    }
}