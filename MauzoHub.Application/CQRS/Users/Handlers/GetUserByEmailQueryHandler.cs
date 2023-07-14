using MauzoHub.Application.CQRS.Users.Queries;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net.Http;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUserByEmailQueryHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetUserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            if (request.Email is null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "400",
                    ErrorMessage = $"user with email {request.Email} already exists",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("A valid email is required to perform this operation: {errorLog}", errorLog);
                throw new BadRequestException("Bad request");
            }

            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "404",
                        ErrorMessage = $"user with email {request.Email} already exists",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };

                    Log.Error($"user with email {request.Email} not found", errorLog);
                    throw new NotFoundException($"user with email {request.Email} not found");
                }

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
