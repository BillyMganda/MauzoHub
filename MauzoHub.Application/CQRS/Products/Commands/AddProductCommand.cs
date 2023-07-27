using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Products.Commands
{
    public class AddProductCommand : IRequest<GetProductsDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new List<string>();
        public Guid BusinessId { get; set; }
    }
}
