using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}
