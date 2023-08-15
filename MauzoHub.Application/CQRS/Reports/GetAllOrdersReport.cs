using MediatR;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetAllOrdersReport : IRequest<IEnumerable<OrderDto>>
    {
    }
}
