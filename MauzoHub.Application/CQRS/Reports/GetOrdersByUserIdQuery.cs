using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
    {
        [Required(ErrorMessage = "userid is required")]
        public Guid UserId { get; set; }
    }
}
