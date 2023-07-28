using MediatR;

namespace MauzoHub.Application.CQRS.Services.Commands
{
    public class DeleteServiceCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
