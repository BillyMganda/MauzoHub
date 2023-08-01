using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IRedisCacheProvider _redisCache;
        public UserRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings, IRedisCacheProvider redisCache)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _usersCollection = database.GetCollection<User>(databaseSettings.Value.UsersCollectionName);

            _redisCache = redisCache;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var cacheKey = $"user_{id}";
            var cachedUser = await _redisCache.GetAsync<User>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                await _redisCache.SetAsync<User>(cacheKey, user, TimeSpan.FromMinutes(5));
            }

            return user!;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var cacheKey = $"user_{email}";
            var cachedUser = await _redisCache.GetAsync<User>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();

            if (user != null)
            {
                await _redisCache.SetAsync<User>(cacheKey, user, TimeSpan.FromMinutes(5));
            }

            return user!;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var cacheKey = "allusers";
            var cachedUser = await _redisCache.GetAsync<IEnumerable<User>>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var users = await _usersCollection.Find(_ => true).ToListAsync();

            if (users != null)
            {
                await _redisCache.SetAsync(cacheKey, users, TimeSpan.FromMinutes(5));
            }

            return users!;
        }

        public async Task<User> AddAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            await _redisCache.RemoveAsync("allusers");
            return user;
        }
        public async Task<User> UpdateAsync(User user)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            // TODO: Work on radis caching 'allusers' key
            await _redisCache.RemoveAsync("allusers");
            return user;
        }

        public async Task<User> DeleteAsync(User user)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == user.Id);
            await _redisCache.RemoveAsync("allusers");
            return user;
        }

        public async Task<bool> DisableUser(Guid Id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var update = Builders<User>.Update.Set(u => u.isActive, false);
            var result = await _usersCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                await _redisCache.RemoveAsync("allusers");
                return true;
            }

            return false;
        }

        public async Task<bool> EnableUser(Guid Id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, Id);
            var update = Builders<User>.Update.Set(u => u.isActive, true);
            var result = await _usersCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                await _redisCache.RemoveAsync("allusers");
                return true;
            }

            return false;
        }

        public async Task<User> GetByTokenAsync(string token)
        {
            var cacheKey = $"user_{token}";
            var cachedUser = await _redisCache.GetAsync<User>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _usersCollection.Find(user => user.PasswordResetToken == token).FirstOrDefaultAsync();

            if (user != null)
            {
                await _redisCache.SetAsync<User>(cacheKey, user, TimeSpan.FromSeconds(5));
            }

            return user!;
        }
    }
}
