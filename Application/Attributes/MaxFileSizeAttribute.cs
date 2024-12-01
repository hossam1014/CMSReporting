using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Attributes
{
    public class MaxFileSizeAttribute: ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var file = value as IFormFile;
            if (file == null)
                return new ValidationResult("Invalid file type");

            if (file.Length > _maxFileSize)
                return new ValidationResult($"Maximum allowed file size is {_maxFileSize / (1024 * 1024)} MB");

            return ValidationResult.Success;
        }
    }
}