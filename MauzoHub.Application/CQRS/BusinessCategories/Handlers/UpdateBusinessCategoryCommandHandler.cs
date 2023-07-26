using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.BusinessCategories.Handlers
{
    public class UpdateBusinessCategoryCommandHandler : IRequestHandler<UpdateBusinessCategoryCommand, GetCategoryDto>
    {
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateBusinessCategoryCommandHandler(IBusinessCategoryRepository businessCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _businessCategoryRepository = businessCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetCategoryDto> Handle(UpdateBusinessCategoryCommand request, CancellationToken cancellationToken)
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
                var category = await _businessCategoryRepository.GetByIdAsync(request.Id);

                if(category == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"user with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("user with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"user with id {request.Id} not found");
                }

                category.CategoryName = request.CategoryName;
                category.LastModified = DateTime.Now;

                await _businessCategoryRepository.UpdateAsync(category);

                var categoryDto = new GetCategoryDto
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                };

                return categoryDto;
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
