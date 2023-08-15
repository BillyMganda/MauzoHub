using MauzoHub.Application.CQRS.Carts.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Carts.Handlers
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductsRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddToCartCommandHandler(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor, IProductsRepository productRepository)
        {
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                // Get the user's cart (create a new cart if not exists)
                var cart = await _cartRepository.GetByUserIdAsync(request.UserId);
                if (cart == null)
                {                    
                    cart = new Cart { UserId = request.UserId, DateCreated = DateTime.Now, LastModified = DateTime.Now };
                }

                // Check if the product exists
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    throw new NotFoundException("Product not found.");
                }

                // Check if the product is already in the cart
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    cart.Items.Add(new CartItem { ProductId = request.ProductId, Quantity = request.Quantity });
                }

                // Save the cart to the repository
                if (cart.Id == Guid.Empty)
                {
                    await _cartRepository.AddAsync(cart);
                }
                else
                {
                    await _cartRepository.UpdateAsync(cart);
                }

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
