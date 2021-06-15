using MySql.Data.MySqlClient;
using ShantyWebAPI.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.CustomAttributes
{
    public class EmailValidation:ValidationAttribute
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
            if(value != null)
            {
                if (IsEmailTaken(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage = "Email Taken");
                }
            }
            return ValidationResult.Success;
        }
    }
}
