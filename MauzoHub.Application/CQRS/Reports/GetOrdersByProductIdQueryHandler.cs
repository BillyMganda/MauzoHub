using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Reports
{
    public class GetOrdersByProductIdQueryHandler : IRequestHandler<GetOrdersByProductIdQuery, IEnumerable<OrderDto>>
    {
        private readonly ICheckoutOrderRepository _checkoutOrderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetOrdersByProductIdQueryHandler(ICheckoutOrderRepository checkoutOrderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _checkoutOrderRepository = checkoutOrderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByProductIdQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var orders = await _checkoutOrderRepository.GetOrdersByProductIdAsync(request.ProductId);

                if(orders is null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"product with id {request.ProductId} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };
                    Log.Error("An error occurred while processing the command, order not found: {@ErrorLog}", errorLog);

                    throw new NotFoundException("order not found");
                }

                var orderDtos = orders.Select(order => new OrderDto
                {
                    Id = order.Id,
                    DateCreated = order.DateCreated,
                    UserId = order.UserId,
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }).ToList()
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
