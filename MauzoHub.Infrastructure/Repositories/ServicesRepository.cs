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

        public async Task<Services> AddAsync(Services entity)
        {
            await _servicesCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Services> DeleteAsync(Services entity)
        {
            await _servicesCollection.DeleteOneAsync(b => b.Id == entity.Id);
            return entity;
        }

        public async Task<IEnumerable<Services>> GetAllAsync()
        {
            var services = await _servicesCollection.Find(s => true).ToListAsync();
            return services;
        }

        public async Task<Services> GetByIdAsync(Guid id)
        {
            var service = await _servicesCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
            return service;
        }

        public async Task<Services> UpdateAsync(Services entity)
        {
            await _servicesCollection.ReplaceOneAsync(s => s.Id == entity.Id, entity);
            return entity;
        }
    }
}
