using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Validations
{
    public class FileWeightValidation : ValidationAttribute
    {
        private readonly int maxWeightMegaBytes;

        public FileWeightValidation(int maxWeightMegaBytes)
        {
            this.maxWeightMegaBytes = maxWeightMegaBytes;
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
            if (file.Length > this.maxWeightMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"The weight of the file must be less than {this.maxWeightMegaBytes} MegaBytes");
            }

            return ValidationResult.Success;
        }
    }
}
