using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersForADateQuery : IRequest<IEnumerable<OrderDto>>
    {
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Today;
    }
}
