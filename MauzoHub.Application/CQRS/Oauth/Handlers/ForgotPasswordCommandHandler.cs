using MauzoHub.Application.CQRS.Oauth.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net.Http;

namespace MauzoHub.Application.CQRS.Oauth.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IOauthRepository _oauthRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForgotPasswordCommandHandler(IOauthRepository oauthRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _oauthRepository = oauthRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            var httpMethod = httpContext.Request.Method;

            var findUserByEmail = await _userRepository.GetByEmailAsync(request.Email);

            if (findUserByEmail is null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "404",
                    ErrorMessage = $"user with email {request.Email} not found",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                    HttpMethod = httpMethod,
                };

                Log.Error("An error occurred while processing the command: {@ErrorLog}", errorLog);
                throw new NotFoundException("404: User not found");
            }

            try
            {
                var token = _oauthRepository.GeneratePasswordResetTokenAsync(request.Email);
                await _oauthRepository.SendPasswordResetTokenEmailAsync(request.Email, token);
                await _oauthRepository.UpdateResetTokenFieldInDatabase(request.Email, token);
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
