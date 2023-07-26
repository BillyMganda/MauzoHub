using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Businesses.Commands
{
    public class AddBusinessCommand : IRequest<GetBusinessDto>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
    }
}
