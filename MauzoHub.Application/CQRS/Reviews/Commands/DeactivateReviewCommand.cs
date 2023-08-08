using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reviews.Commands
{
    public class DeactivateReviewCommand : IRequest<GetReviewDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
