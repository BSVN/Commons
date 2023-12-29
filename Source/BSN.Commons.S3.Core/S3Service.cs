using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using BSN.Commons.S3.Abstraction;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BSN.Commons.Extensions;

namespace BSN.Commons.S3.Core
{
    /// <inheritdoc />
    public class S3Service : IS3Service
    {
        /// <inheritdoc />
        public S3Service(IOptions<S3ServiceOptions> s3ServiceOptions)
        {
            _s3ServiceOptions = s3ServiceOptions.Value;

            _client = new AmazonS3Client(_s3ServiceOptions.AccessKey,
                                         _s3ServiceOptions.SecretKey,
                                         BuildS3Config(_s3ServiceOptions.EndPoint));
        }

        /// <inheritdoc />
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            bucketName = NormalizeBucketName(bucketName);

            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName) != false)
                {
                    return new S3Response()
                    {
                        Status = System.Net.HttpStatusCode.InternalServerError,
                        Message = $"Bucket '{bucketName}' Already Exists"
                    };
                }
                else
                {
                    var putBucketRequest = new PutBucketRequest()
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    PutBucketResponse response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response()
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception S3Ex)
            {
                return new S3Response()
                {
                    Status = S3Ex.StatusCode,
                    Message = S3Ex.Message
                };
            }
            catch (Exception ex)
            {
                return new S3Response()
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        /// <inheritdoc />
        public Task<S3Response> DeleteBucketAsync(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<S3Response> DeleteObjectAsync(string bucket, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<MemoryStream> GetObjectAsync(string bucket, string key)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            };

            GetObjectResponse response = await _client.GetObjectAsync(request);

            return response.ResponseStream.ToMemoryStream();
        }

        /// <inheritdoc />
        public async Task<S3Response> PutObjectAsync(string bucket, string key, Stream @object, string contentType)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = key,
                    ContentType = contentType,
                };

                request.InputStream = @object;
                PutObjectResponse response = await _client.PutObjectAsync(request);

                return new S3Response()
                {
                    Message = response.ResponseMetadata.RequestId,
                    Status = response.HttpStatusCode
                };

            }
            catch (AmazonS3Exception S3Ex)
            {
                return new S3Response()
                {
                    Status = S3Ex.StatusCode,
                    Message = S3Ex.Message
                };
            }
            catch (Exception Ex)
            {
                return new S3Response()
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = Ex.Message
                };
            }
        }

        /// <inheritdoc />
        public async Task<S3Response> PutObjectAsync(string bucket, string key, byte[] @object, string contentType)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = key,
                    ContentType = contentType,
                };

                using (MemoryStream Stream = new MemoryStream(@object))
                {
                    request.InputStream = Stream;
                    PutObjectResponse response = await _client.PutObjectAsync(request);

                    return new S3Response()
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception S3Ex)
            {
                return new S3Response()
                {
                    Status = S3Ex.StatusCode,
                    Message = S3Ex.Message
                };
            }
            catch (Exception Ex)
            {
                return new S3Response()
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = Ex.Message
                };
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckBucketExistanceAsync(string bucketName)
        {
            try
            {
                bucketName = NormalizeBucketName(bucketName);

                await _client.EnsureBucketExistsAsync(bucketName);

                return true;
            }
            catch (AmazonS3Exception S3Ex)
            {
                return false;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool CheckObjectExistance(string bucketName, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public S3Response CreateBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public S3Response CreatePath(IEnumerable<string> path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public S3Response DeleteBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public S3Response DeleteObject(string bucketName, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CredentialModel RequestTempReadToken(string bucket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CredentialModel RequestTempReadToken(string bucket, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Obsolete]
        public BulkCredentialModel RequestTempReadToken(string bucket, IEnumerable<string> kies)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CredentialModel RequestTempWriteToken(string bucket, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [Obsolete]
        public BulkCredentialModel RequestTempWriteToken(string bucket, IEnumerable<string> key)
        {
            throw new NotImplementedException();
        }

        private AmazonS3Config BuildS3Config(string endpoint)
        {
            return new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
                ServiceURL = endpoint, // replace http://localhost:9000 with URL of your MinIO server
                ForcePathStyle = true, // MUST be true to work correctly with MinIO server                
            };
        }

        private string NormalizeBucketName(string bucketName)
        {
            bucketName = bucketName.ToLower();

            if (bucketName[bucketName.Length - 1] != '/')
                bucketName = bucketName + "/";

            return bucketName;
        }


        private readonly S3ServiceOptions _s3ServiceOptions;
        private readonly IAmazonS3 _client;
    }
}