using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MauzoHub.Infrastructure.Databases;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MauzoHub.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IMongoCollection<Review> _reviewsCollection;
        public ReviewRepository(IOptions<MauzoHubDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _reviewsCollection = database.GetCollection<Review>(databaseSettings.Value.ReviewsCollectionName);
        }

        public async Task<Review> FindReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewsCollection.Find(r => r.Id == reviewId).FirstOrDefaultAsync();

            return review;
        }

        public async Task<Review> CreateReview(Guid userId, Guid productOrServiceId, int ratingValue, string comment)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now,
                UserId = userId,
                ProductOrServiceId = productOrServiceId,
                Rating = new Rating(ratingValue),
                Comment = comment,
                IsActive = true,
            };

            await _reviewsCollection.InsertOneAsync(review);
            return review;
        }

        public async Task<Review> DeactivateReview(Guid reviewId)
        {
            var review = await _reviewsCollection.Find(r => r.Id == reviewId).FirstOrDefaultAsync();
            review.MarkAsInactive();
            await _reviewsCollection.ReplaceOneAsync(r => r.Id == review.Id, review);
            return review;
        }
    }
}
