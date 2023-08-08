namespace MauzoHub.Application.DTOs
{
    public class GetReviewDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductOrServiceId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
