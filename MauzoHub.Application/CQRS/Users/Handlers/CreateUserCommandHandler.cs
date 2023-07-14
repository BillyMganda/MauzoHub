﻿using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CustomExceptions;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using MediatR;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GetUserDto>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var findUserByEmail = await _userRepository.GetByEmailAsync(request.Email);

            if (findUserByEmail is not null)
            {
                var errorLog = new ErrorLog
                {
                    DateTime = DateTime.Now,
                    ErrorCode = "400",
                    ErrorMessage = "Bad request!",
                    IPAddress = "127.0.0.1",
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
                    IPAddress = "127.0.0.1",
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
