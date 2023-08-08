using MediatR;

namespace MauzoHub.Application.CQRS.Carts.Commands
{
    public class AddToCartCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
