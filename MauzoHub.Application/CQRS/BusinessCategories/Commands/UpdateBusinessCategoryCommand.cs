using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.BusinessCategories.Commands
{
    public class UpdateBusinessCategoryCommand : IRequest<GetCategoryDto>
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
