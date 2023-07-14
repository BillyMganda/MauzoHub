using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Users.Queries
{
    public class GetUserByIdQuery : IRequest<GetUserDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
