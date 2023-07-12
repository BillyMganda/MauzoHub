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
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        }

        public User GetById(Guid id)
        {
            return _usersCollection.Find(user => user.Id == id).FirstOrDefault();
        }

        public User GetByEmail(string email)
        {
            return _usersCollection.Find(user => user.Email == email).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return _usersCollection.Find(_ => true).ToList();
        }

        public void Add(User user)
        {
            _usersCollection.InsertOne(user);
        }

        public void Update(User user)
        {
            _usersCollection.ReplaceOne(u => u.Id == user.Id, user);
        }

        public void Delete(User user)
        {
            _usersCollection.DeleteOne(u => u.Id == user.Id);
        }
    }
}
