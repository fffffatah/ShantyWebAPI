using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ShantyWebAPI.Providers;
using MySql.Data.MySqlClient;

namespace ShantyWebAPI.CustomAttributes
{
    public class PasswordAuthentication:ValidationAttribute
    {
        string JwtToken { get; set; }

        public PasswordAuthentication(string jwtToken)
        {
            JwtToken = jwtToken;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var jwt =
            Convert.ToBoolean(
                validationContext.ObjectInstance
                    .GetType()
                    .GetProperty(JwtToken)
                    .GetValue(validationContext.ObjectInstance)
                );
            bool CheckPassword(string pass)
            {
                //todo
                return false;
            }
            if (value != null)
            {
                if (!CheckPassword(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage = "Invalid Password");
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage = "Password Required");
            }
            return ValidationResult.Success;
        }
    }
}
