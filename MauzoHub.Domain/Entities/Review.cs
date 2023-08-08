namespace MauzoHub.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid ProductOrServiceId { get; private set; }
        public Rating Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public void UpdateRating(int ratingValue)
        {
            Rating = new Rating(ratingValue);
        }

        public void UpdateComment(string comment)
        {
            Comment = comment;
        }

        public void MarkAsActive()
        {
            IsActive = true;
        }

        public void MarkAsInactive()
        {
            IsActive = false;
        }
    }
}
