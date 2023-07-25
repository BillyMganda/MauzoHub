using MauzoHub.Application.CQRS.BusinessCategories.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.BusinessCategories.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<GetCategoryDto>>
    {
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllCategoriesQueryHandler(IBusinessCategoryRepository businessCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _businessCategoryRepository = businessCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var categories = await _businessCategoryRepository.GetAllAsync();

                var categoriesDto = categories.Select(c => new GetCategoryDto 
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                });

                return categoriesDto;
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
