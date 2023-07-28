using MauzoHub.Application.CQRS.Services.Commands;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Services.Handlers
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, GetServiceDto>
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateServiceCommandHandler(IServicesRepository servicesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _servicesRepository = servicesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetServiceDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            // TODO: Validate the command before processing



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


                var newService = new Domain.Entities.Services
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                    Name = request.Name,
                    Description = request.Description,
                    Images = imageStrings,
                    BusinessId = request.BusinessId,
                };

                await _servicesRepository.AddAsync(newService);

                var serviceDto = new GetServiceDto
                {
                    Id = newService.Id,
                    Name = newService.Name,
                    Description = newService.Description,
                    Images = newService.Images,
                    BusinessId = newService.BusinessId,
                };

                return serviceDto;

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
