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

        public Task<Checkout> AddAsync(Checkout checkout)
        {
            throw new NotImplementedException();
        }

        public Task<Checkout> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Checkout> UpdateAsync(Checkout checkout)
        {
            throw new NotImplementedException();
        }
    }
}
