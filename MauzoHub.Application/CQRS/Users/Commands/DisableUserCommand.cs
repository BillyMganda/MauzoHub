using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Commands
{
    public class DisableUserCommand : IRequest<GetUserDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
