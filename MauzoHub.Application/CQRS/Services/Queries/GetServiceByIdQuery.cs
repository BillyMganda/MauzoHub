using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Services.Queries
{
    public class GetServiceByIdQuery : IRequest<GetServiceDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
