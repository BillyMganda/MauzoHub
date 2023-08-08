namespace MauzoHub.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductOrServiceId { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsActive { get; set; }

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
