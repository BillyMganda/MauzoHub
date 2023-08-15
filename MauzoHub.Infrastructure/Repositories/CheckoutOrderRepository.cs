using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class CheckoutOrderRepository : ICheckoutOrderRepository
    {
        private readonly IMongoCollection<Checkout> _checkoutCollection;
        public CheckoutOrderRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _checkoutCollection = database.GetCollection<Checkout>(databaseSettings.Value.CheckoutCollectionName);
        }

        public Task<IEnumerable<CheckoutOrder>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CheckoutOrder> GetByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CheckoutOrder>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CheckoutOrder>> GetOrdersBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
