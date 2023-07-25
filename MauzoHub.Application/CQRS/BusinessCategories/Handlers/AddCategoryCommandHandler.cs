using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.BusinessCategories.Handlers
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, GetCategoryDto>
    {
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddCategoryCommandHandler(IBusinessCategoryRepository businessCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _businessCategoryRepository = businessCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetCategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            var findByName = await _businessCategoryRepository.FindByName(request.CategoryName);

            if (findByName is not null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "422",
                    ErrorMessage = $"Category with name {request.CategoryName} already exists",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("An error occurred while processing the command: {@ErrorLog}", errorLog);
                throw new UnprocessableEntityException("Category registered");
            }

            try
            {
                var category = new Domain.Entities.BusinessCategories
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                    CategoryName = request.CategoryName,
                };

                await _businessCategoryRepository.AddAsync(category);

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
