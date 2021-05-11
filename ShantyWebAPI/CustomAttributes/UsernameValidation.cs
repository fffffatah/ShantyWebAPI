using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Providers;

namespace ShantyWebAPI.CustomAttributes
{
    public class UsernameValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool IsUsernameTaken(string username)
            {
                MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
                dbConnection.CreateQuery("SELECT COUNT(*) AS \"COUNTER\" FROM users WHERE username='" + username + "'");
                MySqlDataReader reader = dbConnection.DoQuery();
                string counter = "";
                while (reader.Read())
                {
                    counter = reader["COUNTER"].ToString();
                }
                dbConnection.Dispose();
                return !counter.Equals("0");
            }
            if (value != null && IsUsernameTaken(value.ToString()))
            {
                return new ValidationResult(ErrorMessage = "Username Taken");
            } 
            return ValidationResult.Success;
        }
    }
}
