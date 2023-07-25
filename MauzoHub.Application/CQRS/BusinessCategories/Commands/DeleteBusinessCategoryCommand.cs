using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.BusinessCategories.Commands
{
    public class DeleteBusinessCategoryCommand : IRequest<Unit>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
