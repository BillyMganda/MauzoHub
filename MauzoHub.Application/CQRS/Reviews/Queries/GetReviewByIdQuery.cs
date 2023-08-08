using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reviews.Queries
{
    public class GetReviewByIdQuery : IRequest<GetReviewDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
