using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.DTOs;
using MauzoHub.Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDto>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if(user != null)
            {
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

            return null!;
        }
    }
}
