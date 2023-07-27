using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IMongoCollection<Products> _productsCollection;
        private readonly IRedisCacheProvider _redisCache;
        public ProductsRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings, IRedisCacheProvider redisCache)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _productsCollection = database.GetCollection<Products>(databaseSettings.Value.ProductsCollectionName);

            _redisCache = redisCache;
        }

        public async Task<Products> AddAsync(Products entity)
        {
            await _productsCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Products> DeleteAsync(Products entity)
        {
            await _productsCollection.DeleteOneAsync(b => b.Id == entity.Id);
            return entity;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            var products = await _productsCollection.Find(p => true).ToListAsync();
            return products;
        }

        public async Task<Products> GetByIdAsync(Guid id)
        {
            var product = await _productsCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task<Products> UpdateAsync(Products entity)
        {
            await _productsCollection.ReplaceOneAsync(b => b.Id == entity.Id, entity);
            return entity;
        }
    }
}
