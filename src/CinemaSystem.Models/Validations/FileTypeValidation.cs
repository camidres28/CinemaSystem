using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CinemaSystem.Models.Validations
{
    public class FileTypeValidation : ValidationAttribute
    {
        private readonly string[] validTypes;

        public FileTypeValidation(FileTypes fileType)
        {
            switch (fileType)
            {
                case FileTypes.Image:
                    this.validTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
                    break;
                default:
                    break;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!(value is IFormFile))
            {
                return ValidationResult.Success;
            }

            IFormFile file = value as IFormFile;
            if (!this.validTypes.Contains(file.ContentType))
            {
                return new ValidationResult($"The file type must be one of the following: {string.Join(", ", this.validTypes)}");
            }

            return ValidationResult.Success;
        }
    }
}
