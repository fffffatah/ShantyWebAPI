using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Providers;

namespace ShantyWebAPI.CustomAttributes
{
    public class EmailOtpValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool IsEmailTaken(string email)
            {
                MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
                dbConnection.CreateQuery("SELECT COUNT(*) AS \"COUNTER\" FROM users WHERE email='" + email + "'");
                MySqlDataReader reader = dbConnection.DoQuery();
                string counter = "";
                while (reader.Read())
                {
                    counter = reader["COUNTER"].ToString();
                }
                dbConnection.Dispose();
                return !counter.Equals("0");
            }
            if (value != null)
            {
                if (!IsEmailTaken(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage = "Email Not Found");
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage = "Email Required");
            }
            return ValidationResult.Success;
        }
    }
}
