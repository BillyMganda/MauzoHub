using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Businesses.Queries
{
    public class GetAllBusinessesQuery : IRequest<IEnumerable<GetBusinessDto>>
    {
    }
}
