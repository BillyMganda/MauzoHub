using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Services.Commands
{
    public class DeleteServiceCommand : IRequest<Unit>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
