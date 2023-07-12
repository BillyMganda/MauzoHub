using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class CreateUserCommand : IRequest<GetUserDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
