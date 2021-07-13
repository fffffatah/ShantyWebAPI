using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.CustomAttributes
{
    public class AudioFileValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file;
            if (value != null)
            {
                file = (IFormFile)value;
                if (!(file.ContentType == "audio/ogg"))
                {
                    return new ValidationResult(ErrorMessage = "Invalid Audio Type");
                }
            }
            return ValidationResult.Success;
        }
    }
}
