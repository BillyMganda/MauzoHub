using MauzoHub.Application.CQRS.Businesses.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Businesses.Handlers
{
    public class GetAllBusinessesQueryHandler : IRequestHandler<GetAllBusinessesQuery, IEnumerable<GetBusinessDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBusinessRepository _businessRepository;
        public GetAllBusinessesQueryHandler(IHttpContextAccessor httpContextAccessor, IBusinessRepository businessRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _businessRepository = businessRepository;
        }

        public async Task<IEnumerable<GetBusinessDto>> Handle(GetAllBusinessesQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var businesses = await _businessRepository.GetAllAsync();

                var businessDto = businesses.Select(b => new GetBusinessDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    CategoryId = b.CategoryId,
                    OwnerId = b.OwnerId,
                });

                return businessDto;
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
