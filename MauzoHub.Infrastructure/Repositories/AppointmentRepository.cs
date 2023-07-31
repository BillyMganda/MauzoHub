using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IMongoCollection<Appointments> _appointmentsCollection;
        private readonly IRedisCacheProvider _redisCache;
        public AppointmentRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings, IRedisCacheProvider redisCache)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _appointmentsCollection = database.GetCollection<Appointments>(databaseSettings.Value.AppointmentsCollectionName);

            _redisCache = redisCache;
        }

        public async Task<Appointments> AddAsync(Appointments entity)
        {
            await _appointmentsCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Appointments> DeleteAsync(Appointments entity)
        {
            await _appointmentsCollection.DeleteOneAsync(a => a.Id == entity.Id);
            return entity;
        }

        public async Task<IEnumerable<Appointments>> GetAllAsync()
        {
            var results = await _appointmentsCollection.Find(a => true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<Appointments>> GetAppointmentsForuserAsync(Guid userId)
        {
            var results = await _appointmentsCollection.Find(a => a.UserId == userId).ToListAsync();
            return results;
        }

        public async Task<Appointments> GetByIdAsync(Guid id)
        {
            var result = await _appointmentsCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Appointments> UpdateAsync(Appointments entity)
        {
            await _appointmentsCollection.ReplaceOneAsync(a => a.Id == entity.Id, entity);
            return entity;
        }
    }
}
