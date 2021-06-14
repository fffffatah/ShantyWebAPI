using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShantyWebAPI.Models.User;

namespace ShantyWebAPI.Providers
{
    public class JwtAuthenticationProvider
    {
        public string GenerateJsonWebToken(UserLoginResponseModel userLoginResponseModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, userLoginResponseModel.Id),
            };
            var token = new JwtSecurityToken(Environment.GetEnvironmentVariable("JWT_ISSUER"),
              Environment.GetEnvironmentVariable("JWT_ISSUER"),
              claims,
              expires: DateTime.Now.AddMinutes(600),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*public string GetIdFromJwtToken(string jwtToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidateAudience = true,
                ValidAudience = Environment.GetEnvironmentVariable("JWT_ISSUER")
            };
            var claims = handler.ValidateToken(jwtToken, validations, out var tokenSecure);
            return claims.Claims<JwtRegisteredClaimNames>;
        }*/

        public string ValidateToken(string jwtToken)
        {

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = Environment.GetEnvironmentVariable("JWT_ISSUER");
            validationParameters.ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            try
            {
                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
                return principal.FindFirst("Sid").Value;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}