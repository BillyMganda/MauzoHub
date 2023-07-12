using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class UpdateUserCommand : IRequest<GetUserDto>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
