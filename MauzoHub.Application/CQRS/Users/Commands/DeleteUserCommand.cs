using MediatR;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
