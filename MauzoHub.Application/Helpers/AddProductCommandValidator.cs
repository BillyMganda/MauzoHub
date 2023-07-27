using FluentValidation;
using MauzoHub.Application.CQRS.Products.Commands;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MauzoHub.Application.Helpers
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Images)
                .NotNull().WithMessage("Images are required.")
                .Must(images => images.Count <= 5).WithMessage("Only up to 5 image files are allowed.")
                .Must(images => images.All(IsAllowedImageExtension)).WithMessage("Only image files with extensions .jpg, .jpeg, .png, .gif are allowed.");
        }

        private bool IsAllowedImageExtension(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
