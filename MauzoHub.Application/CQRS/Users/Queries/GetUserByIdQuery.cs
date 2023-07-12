using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Queries
{
    public class GetUserByIdQuery : IRequest<GetUserDto>
    {
        public Guid Id { get; set; }
    }
}
