using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
    }
}
