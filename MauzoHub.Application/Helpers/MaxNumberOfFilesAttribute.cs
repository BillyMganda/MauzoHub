using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Application.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxNumberOfFilesAttribute : ValidationAttribute
    {
        private readonly int _maxFiles;

        public MaxNumberOfFilesAttribute(int maxFiles)
        {
            _maxFiles = maxFiles;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;

            if (files != null && files.Count > _maxFiles)
            {
                return new ValidationResult($"Only {_maxFiles} image files are allowed.");
            }

            return ValidationResult.Success;
        }
    }
}
