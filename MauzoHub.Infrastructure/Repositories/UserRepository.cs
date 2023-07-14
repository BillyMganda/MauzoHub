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
        public UserRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _usersCollection = database.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
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
