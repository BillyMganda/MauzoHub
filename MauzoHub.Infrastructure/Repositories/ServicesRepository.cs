using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly IMongoCollection<Services> _servicesCollection;
        private readonly IRedisCacheProvider _redisCache;
        public ServicesRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings, IRedisCacheProvider redisCache)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _servicesCollection = database.GetCollection<Services>(databaseSettings.Value.ServicesCollectionName);

            _redisCache = redisCache;
        }
    }
}
