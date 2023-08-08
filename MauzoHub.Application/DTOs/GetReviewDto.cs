using MauzoHub.Domain.Entities;

namespace MauzoHub.Application.DTOs
{
    public class GetReviewDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductOrServiceId { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
