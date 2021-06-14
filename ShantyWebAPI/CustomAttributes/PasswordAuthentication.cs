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
            Convert.ToString(
                validationContext.ObjectInstance
                    .GetType()
                    .GetProperty(JwtToken)
                    .GetValue(validationContext.ObjectInstance)
                );
            string id = new JwtAuthenticationProvider().ValidateToken(jwt);
            bool CheckPassword(string pass)
            {
                MysqlConnectionProvider dbConnection = new MysqlConnectionProvider();
                dbConnection.CreateQuery("SELECT pass FROM users WHERE id='" + id + "'");
                MySqlDataReader reader = dbConnection.DoQuery();
                string dbPass = "";
                while (reader.Read())
                {
                    dbPass = reader["pass"].ToString();
                }
                dbConnection.Dispose();
                return BCrypt.Net.BCrypt.Verify(pass, dbPass);
            }
            if (id == "") return new ValidationResult(ErrorMessage = "Verification Failed"); ;
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
