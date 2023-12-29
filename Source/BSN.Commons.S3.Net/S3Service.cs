using Amazon;
using Amazon.Auth.AccessControlPolicy;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using BSN.Commons.S3.Abstraction;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSN.Commons.S3.Net
{
    /// <inheritdoc />
    public class S3Service : IS3Service
    {
        public S3Service(IOptions<S3ServiceOptions> s3ServiceOptions)
        {
            _s3ServiceOptions = s3ServiceOptions.Value;

            _s3Client = new AmazonS3Client(_s3ServiceOptions.AccessKey,
                                           _s3ServiceOptions.SecretKey,
                                           GetS3Config(_s3ServiceOptions.EndPoint));
        }

        /// <inheritdoc />
        public Task<bool> CheckBucketExistanceAsync(string bucketName)
        {
            bucketName = bucketName.ToLower();

            return AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        }

        /// <inheritdoc />
        public bool CheckObjectExistance(string bucketName, string key)
        {
            bucketName = bucketName.ToLower();
            key = key.ToLower();

            try
            {
                GetObjectMetadataResponse Result = _s3Client.GetObjectMetadata(bucketName, key);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public S3Response CreateBucket(string bucketName)
        {
            try
            {
                bucketName = bucketName.ToLower();

                if (bucketName[bucketName.Length - 1] != '/')
                    bucketName = bucketName + "/";

                var s3Client = new AmazonS3Client(_s3ServiceOptions.AccessKey,
                                                  _s3ServiceOptions.SecretKey,
                                                  GetS3Config(_s3ServiceOptions.EndPoint));

                bool bucketExists = AmazonS3Util.DoesS3BucketExistV2(s3Client, bucketName);
                if (!bucketExists)
                {
                    PutBucketRequest putBucketRequest = new PutBucketRequest() 
                    { 
                        BucketName = bucketName 
                    };
                    
                    PutBucketResponse Result = s3Client.PutBucket(putBucketRequest);
                    
                    return new S3Response()
                    {
                        Message = Result.ResponseMetadata.RequestId,
                        Status = Result.HttpStatusCode
                    };
                }

                return new S3Response()
                {
                    Message = "Duplicate Bucket",
                    Status = System.Net.HttpStatusCode.Found
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
        }

        /// <inheritdoc />
        public S3Response DeleteObject(string bucketName, string key)
        {
            bucketName = bucketName.ToLower();
            key = key.ToLower();
            try
            {
                var s3Client = new AmazonS3Client(_s3ServiceOptions.AccessKey,
                                                  _s3ServiceOptions.SecretKey,
                                                  GetS3Config(_s3ServiceOptions.EndPoint));

                DeleteObjectResponse Result = s3Client.DeleteObject(bucketName, key);

                return new S3Response()
                {
                    Message = Result.ResponseMetadata.RequestId,
                    Status = Result.HttpStatusCode
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
        }

        /// <inheritdoc />
        public CredentialModel RequestTempReadToken(string bucket)
        {
            bucket = bucket.ToLower();

            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(_s3ServiceOptions.AccessKey,
                                                                                  _s3ServiceOptions.SecretKey,
                                                                                  GetSTSConfig(_s3ServiceOptions.EndPoint));

            Policy Policy = new Policy()
            {
                Version = POLICY_VERSION_DATE,
                Statements =
                new List<Statement>()
                {
                    new Statement(Statement.StatementEffect.Allow)
                    {
                        Id = string.Empty,
                        Actions = new List<ActionIdentifier>() 
                        { 
                            new ActionIdentifier(GET_OBJECT_ACTION) 
                        },
                        Resources = new List<Resource>() 
                        { 
                            new Resource(RESOURCE_PREFIX + bucket + "/*") 
                        }
                    }
                }
            };

            var response = amazonSecurityTokenService.AssumeRole(new AssumeRoleRequest
            {
                DurationSeconds = _s3ServiceOptions.TempTokenLifeTime,
                Policy = Policy.ToJson(),
                RoleArn = DEFAULT_ARN_AWS,
                RoleSessionName = DEFAULT_ROLE_SESSION_NAME,
                ExternalId = DEFAULT_EXTERNAL_ID,
            });

            return new CredentialModel(_s3ServiceOptions.EndPoint,
                                       response.Credentials.AccessKeyId,
                                       response.Credentials.SecretAccessKey,
                                       response.Credentials.SessionToken,
                                       response.Credentials.Expiration,
                                       bucket);
        }

        /// <inheritdoc />
        public CredentialModel RequestTempReadToken(string bucket, string key = null)
        {
            var ResourceName = bucket = bucket.ToLower();

            if (!string.IsNullOrEmpty(key))
            {
                key = key.ToLower();
                ResourceName = BucketPathBuilder.Build(bucket, key).ToLower();
            }

            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(_s3ServiceOptions.AccessKey,
                                                                                  _s3ServiceOptions.SecretKey,
                                                                                  GetSTSConfig(_s3ServiceOptions.EndPoint));

            Policy Policy = new Policy()
            {
                Version = POLICY_VERSION_DATE,
                Statements =
                new List<Statement>()
                {
                    new Statement(Statement.StatementEffect.Allow)
                    {
                        Id = string.Empty,
                        Actions = new List<ActionIdentifier>() 
                        {
                            new ActionIdentifier(GET_OBJECT_ACTION) 
                        },
                        Resources = new List<Resource>() 
                        { 
                            new Resource(RESOURCE_PREFIX + ResourceName) 
                        }
                    }
                }
            };

            var response = amazonSecurityTokenService.AssumeRole(new AssumeRoleRequest
            {
                DurationSeconds = _s3ServiceOptions.TempTokenLifeTime,
                Policy = Policy.ToJson(),
                RoleArn = DEFAULT_ARN_AWS,
                RoleSessionName = DEFAULT_ROLE_SESSION_NAME,
                ExternalId = DEFAULT_EXTERNAL_ID,
            });

            return new CredentialModel(_s3ServiceOptions.EndPoint,
                                       response.Credentials.AccessKeyId,
                                       response.Credentials.SecretAccessKey,
                                       response.Credentials.SessionToken,
                                       response.Credentials.Expiration,
                                       bucket,
                                       key);
        }

        /// <inheritdoc />
        [Obsolete]
        public BulkCredentialModel RequestTempReadToken(string bucket, IEnumerable<string> kies)
        {
            bucket = bucket.ToLower();
            var ResourceNames = new List<string>();
            kies = kies.Select(P => P.ToLower());

            if (kies == null)
            {
                foreach (var Key in kies)
                {
                    ResourceNames.Add(BucketPathBuilder.Build(bucket, Key).ToLower());
                }
            }

            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(_s3ServiceOptions.AccessKey,
                                                                                  _s3ServiceOptions.SecretKey,
                                                                                  GetSTSConfig(_s3ServiceOptions.EndPoint));

            Policy Policy = new Policy()
            {
                Version = POLICY_VERSION_DATE,
                Statements =
                new List<Statement>()
                    {
                        new Statement(Statement.StatementEffect.Allow)
                            {
                                Id = string.Empty,
                                Actions = new List<ActionIdentifier>() { new ActionIdentifier(GET_OBJECT_ACTION) },
                                Resources = ResourceNames.Select(P => new Resource(RESOURCE_PREFIX + P)).ToList()
                            }
                    }
            };

            var response = amazonSecurityTokenService.AssumeRole(new AssumeRoleRequest
            {
                DurationSeconds = _s3ServiceOptions.TempTokenLifeTime,
                Policy = Policy.ToJson(),
                RoleArn = DEFAULT_ARN_AWS,
                RoleSessionName = DEFAULT_ROLE_SESSION_NAME,
                ExternalId = DEFAULT_EXTERNAL_ID,
            });

            return new BulkCredentialModel(_s3ServiceOptions.EndPoint,
                                           response.Credentials.AccessKeyId,
                                           response.Credentials.SecretAccessKey,
                                           response.Credentials.SessionToken,
                                           response.Credentials.Expiration,
                                           bucket,
                                           kies.ToList());
        }

        /// <inheritdoc />
        public CredentialModel RequestTempWriteToken(string bucket, string key = null)
        {
            var ResourceName = bucket = bucket.ToLower();

            if (!string.IsNullOrEmpty(key))
            {
                key = key.ToLower();
                ResourceName = BucketPathBuilder.Build(bucket, key).ToLower();
            }

            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(_s3ServiceOptions.AccessKey,
                                                                                  _s3ServiceOptions.SecretKey,
                                                                                  GetSTSConfig(_s3ServiceOptions.EndPoint));

            Policy Policy = new Policy()
            {
                Version = POLICY_VERSION_DATE,
                Statements =
                new List<Statement>()
                {
                    new Statement(Statement.StatementEffect.Allow)
                    {
                        Id = string.Empty,
                        Actions = new List<ActionIdentifier>() { new ActionIdentifier(PUT_OBJECT_ACTION) },
                        Resources = new List<Resource>() { new Resource(RESOURCE_PREFIX + ResourceName) }
                    }
                }
            };

            var response = amazonSecurityTokenService.AssumeRole(new AssumeRoleRequest
            {
                DurationSeconds = _s3ServiceOptions.TempTokenLifeTime,
                Policy = Policy.ToJson(),
                RoleArn = DEFAULT_ARN_AWS,
                RoleSessionName = DEFAULT_ROLE_SESSION_NAME,
                ExternalId = DEFAULT_EXTERNAL_ID,
            });

            return new CredentialModel(_s3ServiceOptions.EndPoint,
                                       response.Credentials.AccessKeyId,
                                       response.Credentials.SecretAccessKey,
                                       response.Credentials.SessionToken,
                                       response.Credentials.Expiration,
                                       bucket,
                                       key);
        }

        /// <inheritdoc />
        [Obsolete]
        public BulkCredentialModel RequestTempWriteToken(string bucket, IEnumerable<string> kies)
        {
            bucket = bucket.ToLower();
            var ResourceNames = new List<string>();
            kies = kies.Select(P => P.ToLower());

            if (kies == null)
            {
                foreach (var Key in kies)
                {
                    ResourceNames.Add(BucketPathBuilder.Build(bucket, Key).ToLower());
                }
            }

            var amazonSecurityTokenService = new AmazonSecurityTokenServiceClient(_s3ServiceOptions.AccessKey,
                                                                                  _s3ServiceOptions.SecretKey,
                                                                                  GetSTSConfig(_s3ServiceOptions.EndPoint));

            Policy Policy = new Policy()
            {
                Version = POLICY_VERSION_DATE,
                Statements = new List<Statement>()
                {
                    new Statement(Statement.StatementEffect.Allow)
                    {
                        Id = string.Empty,
                        Actions = new List<ActionIdentifier>()
                        {
                            new ActionIdentifier(PUT_OBJECT_ACTION)
                        },
                        Resources = ResourceNames.Select(P => new Resource(RESOURCE_PREFIX + P)).ToList()
                    }
                }
            };

            AssumeRoleResponse response = amazonSecurityTokenService.AssumeRole(new AssumeRoleRequest
            {
                DurationSeconds = _s3ServiceOptions.TempTokenLifeTime,
                Policy = Policy.ToJson(),
                RoleArn = DEFAULT_ARN_AWS,
                RoleSessionName = DEFAULT_ROLE_SESSION_NAME,
                ExternalId = DEFAULT_EXTERNAL_ID,
            });

            return new BulkCredentialModel(_s3ServiceOptions.EndPoint,
                                           response.Credentials.AccessKeyId,
                                           response.Credentials.SecretAccessKey,
                                           response.Credentials.SessionToken,
                                           response.Credentials.Expiration,
                                           bucket,
                                           kies.ToList());
        }

        /// <inheritdoc />
        public Task<S3Response> CreateBucketAsync(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public S3Response DeleteBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<S3Response> DeleteBucketAsync(string bucketName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<S3Response> PutObjectAsync(string bucket, string key, byte[] @object, string contentType)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                ContentType = contentType,
            };

            try
            {
                using (MemoryStream Stream = new MemoryStream(@object))
                {
                    request.InputStream = Stream;
                    PutObjectResponse response = await _s3Client.PutObjectAsync(request);

                    return new S3Response()
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }
            catch (Exception ex)
            {

            }

            return new S3Response()
            {

            };
        }

        /// <inheritdoc />
        public async Task<S3Response> PutObjectAsync(string bucket, string key, Stream @object, string contentType)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                ContentType = contentType,
            };

            request.InputStream = @object;

            PutObjectResponse response = await _s3Client.PutObjectAsync(request);

            return new S3Response()
            {
                Message = response.ResponseMetadata.RequestId,
                Status = response.HttpStatusCode
            };
        }

        /// <inheritdoc />
        public Task<S3Response> DeleteObjectAsync(string bucket, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<MemoryStream> GetObjectAsync(string bucket, string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string GetPreSignedUrl(string bucket, string key)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucket.ToLower(),
                Key = key,
                Expires = DateTime.Now.AddSeconds(_s3ServiceOptions.TempTokenLifeTime),
                Protocol = Protocol.HTTP
            };

            return _s3Client.GetPreSignedURL(request);
        }

        /// <inheritdoc />
        public S3Response CreatePath(IEnumerable<string> pathParts)
        {
            throw new NotImplementedException();
        }

        private AmazonSecurityTokenServiceConfig GetSTSConfig(string endpoint)
        {
            return new AmazonSecurityTokenServiceConfig()
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = endpoint
            };
        }

        private AmazonS3Config GetS3Config(string endpoint)
        {
            return new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
                ServiceURL = endpoint, // replace http://localhost:9000 with URL of your MinIO server
                ForcePathStyle = true, // MUST be true to work correctly with MinIO server,
                UseHttp = true
            };
        }

        private readonly AmazonS3Client _s3Client;
        private readonly S3ServiceOptions _s3ServiceOptions;

        private const string POLICY_VERSION_DATE = "2012-10-17";
        private const string DEFAULT_ARN_AWS = "arn:aws:iam::123456789012:role/demo";
        private const string DEFAULT_ROLE_SESSION_NAME = "Bob";
        private const string GET_OBJECT_ACTION = @"s3:GetObject";
        private const string DEFAULT_EXTERNAL_ID = "1";
        private const string PUT_OBJECT_ACTION = @"s3:PutObject";
        private const string RESOURCE_PREFIX = @"arn:aws:s3:::";
    }
}

