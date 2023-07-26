using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.BusinessCategories.Queries
{
    public class GetCategoryByIdQuery : IRequest<GetCategoryDto>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
