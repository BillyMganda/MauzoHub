﻿using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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
                var user = await _userRepository.GetByIdAsync(request.Id);

                if (user == null)
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

                user!.FirstName = request.FirstName;
                user.LastName = request.LastName;

                await _userRepository.UpdateAsync(user);

                var userDto = new GetUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                return userDto;
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
