using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<GetUserDto>>
    {
    }
}
