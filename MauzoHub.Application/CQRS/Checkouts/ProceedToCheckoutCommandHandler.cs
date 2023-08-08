using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Checkouts
{
    public class ProceedToCheckoutCommandHandler : IRequestHandler<ProceedToCheckoutCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProceedToCheckoutCommandHandler(ICartRepository cartRepository, ICheckoutRepository checkoutRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _checkoutRepository = checkoutRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ProceedToCheckoutCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var cart = await _cartRepository.GetByUserIdAsync(request.UserId);
                if (cart == null || cart.Items.Count == 0)
                {
                    throw new NotFoundException("Cart is empty.");
                }

                // Convert CartItems to CheckoutItems
                var checkoutItems = cart.Items.Select(item => new CheckoutItem(item.ProductId, item.Quantity)).ToList();

                // Create the checkout object
                var checkout = new Checkout
                {
                    UserId = request.UserId,
                    Items = checkoutItems,
                    Payment = request.Payment,
                    ShippingAddress = request.ShippingAddress
                };

                // Save the checkout to the repository
                await _checkoutRepository.AddAsync(checkout);

                // Optional: You can clear the cart after proceeding to checkout
                cart.Items.Clear();
                await _cartRepository.UpdateAsync(cart);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "500",
                    ErrorMessage = ex.Message,
                    IPAddress = remoteIpAddress.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };
                Log.Error(ex, "An error occurred while processing the command: {@ErrorLog}", errorLog);
                throw;
            }
        }
    }
}
