using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<GetProductsDto>>
    {
    }
}
