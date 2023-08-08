using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _cartsCollection;
        public CartRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _cartsCollection = database.GetCollection<Cart>(databaseSettings.Value.CartsCollectionName);

            
        }

        public Task AddAsync(Cart cart)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}
