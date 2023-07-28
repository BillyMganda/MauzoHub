using MauzoHub.Application.CQRS.Services.Queries;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Services.Handlers
{
    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, GetServiceDto>
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetServiceByIdQueryHandler(IServicesRepository servicesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _servicesRepository = servicesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
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
                var service = await _servicesRepository.GetByIdAsync(request.Id);

                if (service == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"service with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("service with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"service with id {request.Id} not found");
                }

                var serviceDto = new GetServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    Images = service.Images,
                    BusinessId = service.BusinessId,
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
