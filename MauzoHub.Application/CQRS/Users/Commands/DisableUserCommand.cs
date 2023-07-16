using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class DisableUserCommand : IRequest<GetUserDto>
    {
        public Guid Id { get; set; }
    }
}
