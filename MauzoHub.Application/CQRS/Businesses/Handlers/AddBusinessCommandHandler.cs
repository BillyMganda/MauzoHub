using MauzoHub.Application.CQRS.Businesses.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Businesses.Handlers
{
    public class AddBusinessCommandHandler : IRequestHandler<AddBusinessCommand, GetBusinessDto>
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddBusinessCommandHandler(IBusinessRepository businessRepository, IHttpContextAccessor httpContextAccessor)
        {
            _businessRepository = businessRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetBusinessDto> Handle(AddBusinessCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            var business = await _businessRepository.GetBusinessByNameAsync(request.Name);

            if (business is not null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "422",
                    ErrorMessage = $"business with name {request.Name} already exists",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("An error occurred while processing the command: {@ErrorLog}", errorLog);
                throw new UnprocessableEntityException("business already registered");
            }

            try
            {
                var newBusiness = new Domain.Entities.Businesses
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                    CategoryId = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid(),
                };

                await _businessRepository.AddAsync(newBusiness);

                var businessDto = new GetBusinessDto
                {
                    Id = newBusiness.Id,
                    Name = newBusiness.Name,
                    Description = newBusiness.Description,
                    CategoryId = newBusiness.CategoryId,
                    OwnerId = newBusiness.OwnerId,                    
                };

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
