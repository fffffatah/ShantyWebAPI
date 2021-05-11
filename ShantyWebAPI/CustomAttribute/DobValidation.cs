using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.CustomAttribute
{
    public class DobValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool IsValidDob(string dob)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - Convert.ToDateTime(dob).Year;
                if (Convert.ToDateTime(dob) > today.AddYears(-age))
                    age--;
                if (age < 13)
                {
                    return false;
                }
                return true;
            }
            if (value != null && IsValidDob(value.ToString()))
            {
                return new ValidationResult("Listener Must be At Least 13 Years Old");
            }
            return ValidationResult.Success;
        }
    }
}
