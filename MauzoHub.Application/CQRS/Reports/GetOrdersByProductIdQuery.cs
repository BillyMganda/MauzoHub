using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersByProductIdQuery : IRequest<IEnumerable<OrderDto>>
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
