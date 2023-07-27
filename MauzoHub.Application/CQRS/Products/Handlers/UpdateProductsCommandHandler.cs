using MauzoHub.Application.CQRS.Products.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Products.Handlers
{
    public class UpdateProductsCommandHandler : IRequestHandler<UpdateProductsCommand, GetProductsDto>
    {
        private readonly IProductsRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateProductsCommandHandler(IProductsRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetProductsDto> Handle(UpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            if (request.Id == Guid.Empty)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "400",
                    ErrorMessage = "Bad request",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("An error occurred while processing the command, Invalid request Id: {@ErrorLog}", errorLog);
                throw new BadRequestException("Invalid request Id");
            }

            try
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"product with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("product with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"product with id {request.Id} not found");
                }

                product.Name = request.Name;
                product.Description = request.Description;
                product.LastModified = DateTime.Now;

                await _productRepository.UpdateAsync(product);

                var productDto = new GetProductsDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Images = product.Images,
                    BusinessId = product.BusinessId,
                };

                return productDto;
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
