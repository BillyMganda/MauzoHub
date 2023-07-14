﻿using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GetUserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            var actionUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";

            var findUserByEmail = await _userRepository.GetByEmailAsync(request.Email);

            if (findUserByEmail is not null)
            {              
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "400",
                    ErrorMessage = "Bad request",
                    IPAddress = remoteIpAddress!.ToString(),
                    ActionUrl = actionUrl,
                };
                Log.Error("An error occurred while processing the command: {@ErrorLog}", errorLog);

                throw new BadRequestException("Email registered");
            }

            
            try
            {                

                (string salt, string hash) = GenerateSaltAndHash(request.Password);

                var user = new User(request.FirstName, request.LastName, request.Email, salt, hash, request.Role);

                await _userRepository.AddAsync(user);

                var userDto = new GetUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
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
                };
                Log.Error(ex, "An error occurred while processing the command: {@ErrorLog}", errorLog);                

                throw;
            }
        }       

        private (string salt, string hash) GenerateSaltAndHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = Convert.ToBase64String(hashBytes);

                return (salt, hash);
            }
        }
    }
}
