using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> FindReviewByIdAsync(Guid reviewId);
        Task<Review> CreateReview(Guid userId, Guid productOrServiceId, int ratingValue, string comment);
        Task<Review> DeactivateReview(Guid reviewId);
        Task<Review> ActivateReview(Guid reviewId);
    }
}
