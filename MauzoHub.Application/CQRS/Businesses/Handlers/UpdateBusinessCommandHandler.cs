﻿using MauzoHub.Application.CQRS.Businesses.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Businesses.Handlers
{
    public class UpdateBusinessCommandHandler : IRequestHandler<UpdateBusinessCommand, GetBusinessDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBusinessRepository _businessRepository;
        public UpdateBusinessCommandHandler(IHttpContextAccessor httpContextAccessor, IBusinessRepository businessRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _businessRepository = businessRepository;
        }

        public async Task<GetBusinessDto> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
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
                var business = await _businessRepository.GetByIdAsync(request.Id);

                if (business == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"business with id {request.Id} not found",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error("business with provided id not found: {errorLog}", errorLog);
                    throw new NotFoundException($"business with id {request.Id} not found");
                }               

                // TODO: Check on this
                //var avoidName = await _businessRepository.GetBusinessByNameAsync(request.Name);

                //if (avoidName != null)
                //{
                //    var errorLog = new ErrorLog
                //    {
                //        DateTime = DateTime.Now,
                //        ErrorCode = "400",
                //        ErrorMessage = $"business with name {request.Name} already exists",
                //        IPAddress = remoteIpAddress!.ToString(),
                //        ActionUrl = actionUrl,
                //        HttpMethod = httpMethod,
                //    };

                //    Log.Error("business with provided name exists: {errorLog}", errorLog);
                //    throw new BadRequestException($"business with name {request.Name} exists");
                //}

                business.Name = request.Name;
                business.Description = request.Description;
                business.CategoryId = request.CategoryId;
                business.LastModified = DateTime.Now;

                await _businessRepository.UpdateAsync(business);

                var businessDto = new GetBusinessDto
                {
                    Id = business.Id,
                    Name = business.Name,
                    Description = business.Description,
                    CategoryId = business.CategoryId,
                    OwnerId = business.OwnerId,
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
