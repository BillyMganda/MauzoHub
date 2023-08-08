using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Review CreateReview(Guid userId, Guid productOrServiceId, int ratingValue, string comment);
        void DeactivateReview(Guid reviewId);
    }
}
