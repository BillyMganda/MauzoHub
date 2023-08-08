using MauzoHub.Application.CQRS.Reviews.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Reviews.Handlers
{
    public class GetReviewsForProductOrServiceQueryHandler : IRequestHandler<GetReviewsForProductOrServiceQuery, IEnumerable<GetReviewDto>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetReviewsForProductOrServiceQueryHandler(IReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetReviewDto>> Handle(GetReviewsForProductOrServiceQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var reviews = await _reviewRepository.GetReviewsForProductOrService(request.id);

                var reviewDto = reviews.Select(r => new GetReviewDto 
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ProductOrServiceId = r.ProductOrServiceId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                });

                return reviewDto;
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
