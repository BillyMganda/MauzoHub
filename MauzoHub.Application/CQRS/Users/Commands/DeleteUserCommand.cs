using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
