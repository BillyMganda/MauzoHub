using MauzoHub.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MauzoHub.Application.CQRS.Products.Commands
{
    public class AddProductCommand : IRequest<GetProductsDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public Guid BusinessId { get; set; }
    }
}
