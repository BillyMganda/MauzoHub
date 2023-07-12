using MauzoHub.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Databases
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("MongoDbConnection")!;
            string databaseName = configuration.GetSection("DatabaseSettings")["DatabaseName"]!;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
