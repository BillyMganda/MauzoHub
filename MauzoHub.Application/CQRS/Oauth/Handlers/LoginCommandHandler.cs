﻿using MauzoHub.Application.CQRS.Oauth.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MauzoHub.Application.CQRS.Oauth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOauthRepository _oauthRepository;
        public LoginCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IOauthRepository oauthRepository)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _oauthRepository = oauthRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
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
                    ErrorMessage = $"User with email {request.Email} not found",
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
                    // Successful Login
                    string role = user.Role;
                    var accessToken = _oauthRepository.CreateJwtToken(request.Email, role);
                    var refreshToken = _oauthRepository.GenerateRefreshToken();

                    // Save Refresh token To database
                    var refresh_token = new RefreshTokens
                    {
                        Id = Guid.NewGuid(),
                        Email = request.Email,
                        Token = refreshToken,
                        ExpiryDate = DateTime.Now.AddDays(7),
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                    };
                    await _oauthRepository.SaveRefreshRoken(refresh_token);                    


                    var loginResponse = new LoginResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    };

                    return loginResponse;
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
