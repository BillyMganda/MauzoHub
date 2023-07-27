using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Products.Queries
{
    public class GetProductByIdQuery : IRequest<GetProductsDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}

