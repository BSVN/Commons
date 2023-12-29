using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BSN.Commons.S3.Abstraction
{
    /// <summary>
    /// S3 Service interface for communicating with AmazonS3 compatible object-storages.
    /// </summary>
    public interface IS3Service
    {
        /// <summary>
        /// Check whether the bucket is exist in s3 server or not.
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <returns>True if exists.</returns>
        Task<bool> CheckBucketExistanceAsync(string bucketName);

        /// <summary>
        /// Check whether the object is exist in s3 server or not.
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <param name="key">Object key</param>
        /// <returns>True if exists.</returns>
        bool CheckObjectExistance(string bucketName, string key);

        /// <summary>
        /// Create a bucket.
        /// <remarks>
        /// There are a lots of rule on naming buckets, 
        /// please visit below link to keep your codes from possible miss-functioning.
        /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/bucketnamingrules.html"/>
        /// </remarks>
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        S3Response CreateBucket(string bucketName);

        /// <summary>
        /// Create a bucket.
        /// <remarks>
        /// There are a lots of rule on naming buckets, 
        /// please visit below link to keep your codes from possible miss-functioning.
        /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/bucketnamingrules.html"/>
        /// </remarks>
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<S3Response> CreateBucketAsync(string bucketName);

        /// <summary>
        /// Delete an existing bucket.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        S3Response DeleteBucket(string bucketName);

        /// <summary>
        /// Delete an existing bucket.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<S3Response> DeleteBucketAsync(string bucketName);

        /// <summary>
        /// Put an object into the s3 server.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <param name="object">Object in byte array representation.</param>
        /// <param name="contentType">Content MimeType (Mimetype is more important than a possible extension).</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<S3Response> PutObjectAsync(string bucket, string key, byte[] @object, string contentType);

        /// <summary>
        /// Put an object into the s3 server.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <param name="object">Object in stream representation.</param>
        /// <param name="contentType">Content MimeType (Mimetype is more important than a possible extension).</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<S3Response> PutObjectAsync(string bucket, string key, Stream @object, string contentType);

        /// <summary>
        /// Delete an exsisting bucket from s3 server.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        S3Response DeleteObject(string bucketName, string key);

        /// <summary>
        /// Delete an exsisting bucket from s3 server.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<S3Response> DeleteObjectAsync(string bucket, string key);

        /// <summary>
        /// Create a path using a collection of bucket names.
        /// </summary>
        /// <param name="pathParts">Path parts.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        S3Response CreatePath(IEnumerable<string> pathParts);

        /// <summary>
        /// Get an exsiting object.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <returns>Http status code should have a 200-299 value for successful call.</returns>
        Task<MemoryStream> GetObjectAsync(string bucket, string key);

        /// <summary>
        /// Request a temp token for reading an specific bucket's contents for external s3 user.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <returns>S3 Service credential model.</returns>
        CredentialModel RequestTempReadToken(string bucket);

        /// <summary>
        /// Request a temp token for reading an specific object for external s3 user.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="key">Object key.</param>
        /// <returns>S3 Service credential model.</returns>
        CredentialModel RequestTempReadToken(string bucket, string key);

        /// <summary>
        /// Request a temp token for reading several objects for external s3 user.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="kies">Object kies.</param>
        /// <returns>S3 Service credential model.</returns>
        [Obsolete("[R.Noei] I'm not sure if we need this feature anymore.")]
        BulkCredentialModel RequestTempReadToken(string bucket, IEnumerable<string> kies);

        /// <summary>
        /// Request a temp token for writing an specific object for external s3 user.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="kies">Object key.</param>
        /// <returns>S3 Service credential model.</returns>
        CredentialModel RequestTempWriteToken(string bucket, string key);

        /// <summary>
        /// Request a temp token for writing several objects for external s3 user.
        /// </summary>
        /// <param name="bucket">Bucket name.</param>
        /// <param name="kies">Object kies.</param>
        /// <returns>S3 Service credential model.</returns>
        [Obsolete("[R.Noei] I'm not sure if we need this feature anymore.")]
        BulkCredentialModel RequestTempWriteToken(string bucket, IEnumerable<string> kies);
    }
}