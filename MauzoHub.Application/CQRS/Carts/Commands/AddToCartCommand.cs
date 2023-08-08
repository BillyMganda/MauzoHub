using MediatR;

namespace MauzoHub.Application.CQRS.Carts.Commands
{
    public class AddToCartCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
