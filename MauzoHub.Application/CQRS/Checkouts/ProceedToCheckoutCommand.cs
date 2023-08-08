using MauzoHub.Domain.Entities;
using MediatR;

namespace MauzoHub.Application.CQRS.Checkouts
{
    public class ProceedToCheckoutCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Payment Payment { get; set; } = new Payment();
        public ShippingAddress ShippingAddress { get; set; } = new ShippingAddress();
    }
}
