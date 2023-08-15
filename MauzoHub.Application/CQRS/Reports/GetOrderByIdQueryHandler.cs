using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly ICheckoutOrderRepository _checkoutOrderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetOrderByIdQueryHandler(ICheckoutOrderRepository checkoutOrderRepository, IHttpContextAccessor contextAccessor)
        {
            _checkoutOrderRepository = checkoutOrderRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var order = await _checkoutOrderRepository.GetByIdAsync(request.OrderId);

                if(order is null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"order with orderid {request.OrderId} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };
                    Log.Error("An error occurred while processing the command, order not found: {@ErrorLog}", errorLog);

                    throw new NotFoundException("order not found");
                }

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    DateCreated = order.DateCreated,
                    UserId = order.UserId,
                    Items = order.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                    }).ToList(),
                    Payment = new PaymentDto
                    {
                        PaymentMethod = order.Payment.PaymentMethod,
                        CardNumber = order.Payment.CardNumber,
                    },
                    ShippingAddress = new ShippingAddressDto
                    {
                        Street = order.ShippingAddress.Street,
                        City = order.ShippingAddress.City,
                        Country = order.ShippingAddress.Country,
                    }
                };

                return orderDto;
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
 