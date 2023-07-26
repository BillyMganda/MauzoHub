using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Businesses.Queries
{
    public class GetBusinessByIdQuery : IRequest<GetBusinessDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
