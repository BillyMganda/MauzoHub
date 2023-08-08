using MauzoHub.Application.CQRS.Reviews.Commands;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Reviews.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, GetReviewDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var review = await _reviewRepository.CreateReview(request.UserId, request.ProductOrServiceId, request.Rating, request.Comment);
                
                var reviewDto = new GetReviewDto
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    ProductOrServiceId = review.ProductOrServiceId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                };

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
