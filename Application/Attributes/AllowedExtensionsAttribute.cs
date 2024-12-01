using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var file = value as IFormFile;
            if (file == null)
                return new ValidationResult("Invalid file type");

            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
            if (!_extensions.Contains(extension))
                return new ValidationResult($"Only the following file types are allowed: {string.Join(", ", _extensions)}");

            return ValidationResult.Success;
        }
    }
}