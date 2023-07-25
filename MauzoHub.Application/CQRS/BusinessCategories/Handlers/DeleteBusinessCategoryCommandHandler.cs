using MauzoHub.Application.CQRS.BusinessCategories.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.BusinessCategories.Handlers
{
    public class DeleteBusinessCategoryCommandHandler : IRequestHandler<DeleteBusinessCategoryCommand, Unit>
    {
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteBusinessCategoryCommandHandler(IBusinessCategoryRepository businessCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _businessCategoryRepository = businessCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(DeleteBusinessCategoryCommand request, CancellationToken cancellationToken)
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

                if (category == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"category with id {request.Id} not found",
                        IPAddress = remoteIpAddress.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error($"category with id {request.Id} not found", errorLog);
                    throw new NotFoundException($"category with id {request.Id} not found");
                }

                await _businessCategoryRepository.DeleteAsync(category);

                return Unit.Value;

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
