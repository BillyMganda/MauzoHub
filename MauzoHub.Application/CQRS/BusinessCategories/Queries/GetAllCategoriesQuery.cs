using MauzoHub.Application.DTOs;
using MediatR;

namespace MauzoHub.Application.CQRS.BusinessCategories.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<GetCategoryDto>>
    {
    }
}
