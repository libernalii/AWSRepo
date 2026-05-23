using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CinemaCore.Models;
using Microsoft.Extensions.Configuration;

namespace CinemaStorage.Services
{
    [DynamoDBTable("CinemaUsers")]
    public class DynamoUserEntity
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string FavoriteGenre { get; set; }
    }

    public class DynamoDbService
    {
        private readonly DynamoDBContext _context;

        public DynamoDbService(IAmazonDynamoDB dynamoDb)
        {
            _context = new DynamoDBContext(dynamoDb);
        }

        public async Task SaveUserAsync(DynamoUser user)
        {
            var entity = new DynamoUserEntity
            {
                UserId = user.UserId,
                Name = user.Name,
                FavoriteGenre = user.FavoriteGenre
            };

            await _context.SaveAsync(entity);
        }

        public async Task<DynamoUser> GetUserAsync(string userId)
        {
            var entity = await _context.LoadAsync<DynamoUserEntity>(userId);

            if (entity == null)
                return null;

            return new DynamoUser
            {
                UserId = entity.UserId,
                Name = entity.Name,
                FavoriteGenre = entity.FavoriteGenre
            };
        }
    }
}