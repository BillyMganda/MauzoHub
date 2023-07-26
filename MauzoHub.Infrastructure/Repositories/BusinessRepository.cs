using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly IMongoCollection<Businesses> _businessCollection;
        public BusinessRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _businessCollection = database.GetCollection<Businesses>(databaseSettings.Value.BusinessCollectionName);
        }

        public async Task<Businesses> AddAsync(Businesses entity)
        {
            await _businessCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Businesses> DeleteAsync(Businesses entity)
        {
            await _businessCollection.DeleteOneAsync(b => b.Id == entity.Id);
            return entity;
        }

        public async Task<IEnumerable<Businesses>> GetAllAsync()
        {
            var businesses = await _businessCollection.Find(b => true).ToListAsync();
            return businesses;
        }

        public async Task<Businesses> GetByIdAsync(Guid id)
        {
            var business = await _businessCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
            return business;
        }

        public async Task<Businesses> UpdateAsync(Businesses entity)
        {
            await _businessCollection.ReplaceOneAsync(b => b.Id == entity.Id, entity);
            return entity;
        }
    }
}
