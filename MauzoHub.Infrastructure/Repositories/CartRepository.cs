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

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _cartsCollection
                .InsertOneAsync(cart);
            return cart;
        }

        public async Task<Cart> GetByUserIdAsync(Guid userId)
        {
            var cart = await _cartsCollection
                .Find(c => c.UserId == userId)
                .FirstOrDefaultAsync();
            return cart;
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            await _cartsCollection
                .ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return cart;
        }
    }
}
