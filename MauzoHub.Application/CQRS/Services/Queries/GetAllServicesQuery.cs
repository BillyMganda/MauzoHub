using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Services.Queries
{
    public class GetAllServicesQuery : IRequest<IEnumerable<GetServiceDto>>
    {
    }
}
