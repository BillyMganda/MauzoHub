using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Businesses.Commands
{
    public class DeleteBusinessCommand : IRequest<Unit>
    {
        [Required]        
        public Guid Id { get; set; }
    }
}
