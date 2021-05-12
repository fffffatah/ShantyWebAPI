using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.CustomAttributes
{
    public class ImageValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                IFormFile file = (IFormFile)value;
                if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png"))
                {
                    return new ValidationResult(ErrorMessage = "Invalid Image Type");
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage = "Profile Image Required");
            }
            return ValidationResult.Success;
        }
    }
}
