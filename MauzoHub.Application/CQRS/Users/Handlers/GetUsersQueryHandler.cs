using MauzoHub.Application.CQRS.Users.Queries;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net.Http;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUsersQueryHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<GetUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            try
            {
                var users = await _userRepository.GetAllAsync();

                var userDto = users.Select(u => new GetUserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                });

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
