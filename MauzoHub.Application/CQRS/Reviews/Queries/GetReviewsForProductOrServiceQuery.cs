using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reviews.Queries
{
    public class GetReviewsForProductOrServiceQuery : IRequest<IEnumerable<GetReviewDto>>
    {
        [Required]
        public Guid ProductOrServiceId { get; set; }
    }
}
