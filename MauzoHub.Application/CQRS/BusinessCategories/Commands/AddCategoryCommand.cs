using MauzoHub.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.CQRS.BusinessCategories.Commands
{
    public class AddCategoryCommand : IRequest<GetCategoryDto>
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}
