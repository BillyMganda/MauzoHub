using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Queries
{
    public class GetUserByEmailQuery : IRequest<GetUserDto>
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
