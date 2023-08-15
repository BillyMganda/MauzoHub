using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Reports
{
    internal class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
    {
        [Required(ErrorMessage = "variable userid is required")]
        public Guid UserId { get; set; }
    }
}
