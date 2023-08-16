using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
    {
        private readonly ICheckoutOrderRepository _checkoutOrderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetOrdersByUserIdQueryHandler(ICheckoutOrderRepository checkoutOrderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _checkoutOrderRepository = checkoutOrderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var reports = await _checkoutOrderRepository.GetByUserIdAsync(request.UserId);

                var orderDtos = reports.Select(r => new OrderDto
                {
                    Id = r.Id,
                    DateCreated = r.DateCreated,
                    UserId = r.UserId,
                    Items = r.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                    }).ToList(),
                    Payment = new PaymentDto
                    {
                        PaymentMethod = r.Payment.PaymentMethod,
                        CardNumber = r.Payment.CardNumber
                    },
                    ShippingAddress = new ShippingAddressDto
                    {
                        Street = r.ShippingAddress.Street,
                        City = r.ShippingAddress.City,
                        Country = r.ShippingAddress.Country
                    }
                }).ToList();

                return orderDtos;
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
