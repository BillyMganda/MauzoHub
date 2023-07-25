using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class BusinessCategoryRepository : IBusinessCategoryRepository
    {
        private readonly IMongoCollection<BusinessCategories> _businessCategoriesCollection;
        public BusinessCategoryRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _businessCategoriesCollection = database.GetCollection<BusinessCategories>(databaseSettings.Value.BusinessCategoriesCollectionName);
        }
        public async Task<BusinessCategories> AddAsync(BusinessCategories entity)
        {
            await _businessCategoriesCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<BusinessCategories> DeleteAsync(BusinessCategories entity)
        {
            await _businessCategoriesCollection.DeleteOneAsync(u => u.Id == entity.Id);            
            return entity;
        }

        public async Task<IEnumerable<BusinessCategories>> GetAllAsync()
        {
            var categories = await _businessCategoriesCollection.Find(_ => true).ToListAsync();
            return categories;
        }

        public async Task<BusinessCategories> GetByIdAsync(Guid id)
        {
            var category = await _businessCategoriesCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<BusinessCategories> FindByName(string name)
        {
            var category = await _businessCategoriesCollection.Find(c => c.CategoryName == name).FirstOrDefaultAsync();
            return category;
        }

        public async Task<BusinessCategories> UpdateAsync(BusinessCategories entity)
        {
            await _businessCategoriesCollection.ReplaceOneAsync(c => c.Id == entity.Id, entity);
            return entity;
        }
    }
}
