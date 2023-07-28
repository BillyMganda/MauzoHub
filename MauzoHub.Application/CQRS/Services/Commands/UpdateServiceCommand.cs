using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Services.Commands
{
    public class UpdateServiceCommand : IRequest<GetServiceDto>
    {
        [Required]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
