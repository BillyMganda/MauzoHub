using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MauzoHub.Application.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();

                        if (!_allowedExtensions.Contains(fileExtension))
                        {
                            return new ValidationResult($"Only image files with extensions {_allowedExtensions} are allowed.");
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
