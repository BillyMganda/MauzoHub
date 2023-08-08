using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly IMongoCollection<Checkout> _checkoutCollection;
        public CheckoutRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _checkoutCollection = database.GetCollection<Checkout>(databaseSettings.Value.CheckoutCollectionName);
        }

        public async Task<Checkout> AddAsync(Checkout checkout)
        {
            await _checkoutCollection
                .InsertOneAsync(checkout);
            return checkout;
        }

        public async Task<Checkout> GetByUserIdAsync(Guid userId)
        {
            var cart = await _checkoutCollection
                .Find(c => c.UserId == userId)
                .FirstOrDefaultAsync();
            return cart;
        }

        public async Task<Checkout> UpdateAsync(Checkout checkout)
        {
            await _checkoutCollection
                .ReplaceOneAsync(c => c.Id == checkout.Id, checkout);
            return checkout;
        }
    }
}
