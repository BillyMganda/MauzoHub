using MediatR;

namespace MauzoHub.Application.CQRS.BusinessCategories.Commands
{
    public class DeleteBusinessCategoryCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
