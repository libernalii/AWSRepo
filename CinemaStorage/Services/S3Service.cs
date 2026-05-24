using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CinemaStorage.Services
{
    public class S3Service
    {
        private readonly IConfiguration _configuration;
        private readonly AmazonS3Client _client;

        public S3Service(IConfiguration configuration)
        {
            _configuration = configuration;

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };

            // Клієнт автоматично шукає IAM-роль сервера EC2
            _client = new AmazonS3Client(config);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var bucketName = _configuration["AWS:BucketName"];
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            using var stream = file.OpenReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = bucketName,
                ContentType = file.ContentType
            };

            var transferUtility = new TransferUtility(_client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"https://{bucketName}.s3.amazonaws.com/{fileName}";
        }
    }
}