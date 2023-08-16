using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersBetweenDatesQuery : IRequest<IEnumerable<OrderDto>>
    {
        public DateTime DateFrom { get; set; } = DateTime.Today;
        public DateTime DateTo { get; set; } = DateTime.Today;
    }
}
