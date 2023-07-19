using MauzoHub.Application.CQRS.Oauth.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text;

namespace MauzoHub.Application.CQRS.Oauth.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOauthRepository _oauthRepository;
        public UserLoginCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IOauthRepository oauthRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _oauthRepository = oauthRepository;
        }

        public async Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;


            var user = await _userRepository.GetByEmailAsync(request.Email);

            if(user == null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "404",
                    ErrorMessage = "User not found",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };
                Log.Error("An error occurred while processing the command, user not found: {@ErrorLog}", errorLog);

                throw new NotFoundException("User not found");
            }

            if (user.isActive == false)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "403",
                    ErrorMessage = "User not active",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };
                Log.Error("An error occurred while processing the command, user inactive: {@ErrorLog}", errorLog);

                throw new ForbiddenException("User not active");
            }

            try
            {
                // verify password    
                var isPasswordCorrect = _oauthRepository.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

                if(isPasswordCorrect == false)
                {
                    var errorLog = new ErrorLog
                    {
                        DateTime = DateTime.Now,
                        ErrorCode = "400",
                        ErrorMessage = "invalid password",
                        IPAddress = remoteIpAddress!.ToString(),
                        ActionUrl = actionUrl,
                        HttpMethod = httpMethod,
                    };
                    Log.Error("An error occurred while processing the command, invalid password: {@ErrorLog}", errorLog);

                    throw new BadRequestException("Invalid email or password");
                }
                else
                {
                    var Token = _oauthRepository.CreateJwtToken(request.Email);

                    return Token;
                }                
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
