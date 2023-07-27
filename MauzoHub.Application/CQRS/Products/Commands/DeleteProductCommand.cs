using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
