using MauzoHub.Application.CQRS.Services.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Services.Handlers
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<GetServiceDto>>
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllServicesQueryHandler(IServicesRepository servicesRepository, IHttpContextAccessor contextAccessor)
        {
            _servicesRepository = servicesRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<GetServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var services = await _servicesRepository.GetAllAsync();

                var servicesDto = services.Select(s => new GetServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Images = s.Images,
                    BusinessId = s.BusinessId,
                });

                return servicesDto;
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
