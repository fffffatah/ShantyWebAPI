using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ShantyWebAPI.Providers;

namespace ShantyWebAPI.CustomAttributes
{
    public class PhoneValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool IsPhoneTaken(string phone)
            {
                MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
                dbConnection.CreateQuery("SELECT COUNT(*) AS \"COUNTER\" FROM users WHERE phone='" + phone + "'");
                MySqlDataReader reader = dbConnection.DoQuery();
                string counter = "";
                while (reader.Read())
                {
                    counter = reader["COUNTER"].ToString();
                }
                dbConnection.Dispose();
                return !counter.Equals("0");
            }
            if (value != null && IsPhoneTaken(value.ToString()))
            {
                return new ValidationResult(ErrorMessage = "Phone Taken");
            }
            return ValidationResult.Success;
        }
    }
}
