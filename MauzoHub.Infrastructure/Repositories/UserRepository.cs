﻿using MauzoHub.Domain.Entities;
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
            var cacheKey = "users";
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
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return user;
        }

        public async Task<User> DeleteAsync(User user)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == user.Id);
            return user;
        }
    }
}
