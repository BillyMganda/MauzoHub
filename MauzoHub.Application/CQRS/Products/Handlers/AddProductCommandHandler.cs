using MauzoHub.Application.CQRS.Products.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Application.Helpers;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Products.Handlers
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, GetProductsDto>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddProductCommandHandler(IProductsRepository productsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productsRepository = productsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetProductsDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            // Validate the command before processing
            var validationResult = await new AddProductCommandValidator().ValidateAsync(request);

            if (!validationResult.IsValid)
            {                
                throw new ValidationException("validation failed");
            }


            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                // Handle image uploads and convert them to strings
                var imageStrings = new List<string>();
                foreach (var imageFile in request.Images)
                {
                    if (imageFile.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await imageFile.CopyToAsync(memoryStream);
                        imageStrings.Add(Convert.ToBase64String(memoryStream.ToArray()));
                    }
                }

                var newProduct = new Domain.Entities.Products
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Images = imageStrings,
                    BusinessId = request.BusinessId,
                };

                await _productsRepository.AddAsync(newProduct);

                var getProductDto = new GetProductsDto
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description= newProduct.Description,
                    Price= newProduct.Price,
                    Images= newProduct.Images,
                    BusinessId= newProduct.BusinessId,
                };

                return getProductDto;
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
