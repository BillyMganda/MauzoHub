using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reviews.Commands
{
    public class CreateReviewCommand : IRequest<GetReviewDto>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductOrServiceId { get; set; }
        [Required]
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
