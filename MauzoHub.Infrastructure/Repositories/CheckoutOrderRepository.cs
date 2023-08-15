using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class CheckoutOrderRepository : ICheckoutOrderRepository
    {
        private readonly IMongoCollection<CheckoutOrder> _checkoutCollection;
        public CheckoutOrderRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _checkoutCollection = database.GetCollection<CheckoutOrder>(databaseSettings.Value.CheckoutCollectionName);
        }

        public async Task<IEnumerable<CheckoutOrder>> GetAllAsync()
        {
            var orders = await _checkoutCollection.Find(o => true).ToListAsync();
            return orders;
        }

        public async Task<CheckoutOrder> GetByIdAsync(Guid orderId)
        {
            var order = await _checkoutCollection.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            return order;
        }

        public async Task<IEnumerable<CheckoutOrder>> GetByUserIdAsync(Guid userId)
        {
            var orders = await _checkoutCollection.Find(o => o.UserId == userId).ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<CheckoutOrder>> GetOrdersBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _checkoutCollection.Find(o => o.DateCreated >= startDate && o.DateCreated <= endDate).ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<CheckoutOrder>> GetOrdersForADateAsync(DateTime date)
        {
            return await _checkoutCollection.Find(o => o.DateCreated == date).ToListAsync();
        }
    }
}
