using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IMMPlaH.Services
{
    public class AuthentificationService
    {
        public string SecretKey = "TW9zaGVFcmV6UHJpdmF0ZUtleQ==";
        public int ExpireMinutes = 10080;
        public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256Signature;

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(SecretKey))
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken(Claim[] claim)
        {
            /*var x = ConfigurationManager.AppSettings;
            SecretKey = ConfigurationManager.AppSettings["SecretKey"];*/

            if (claim == null)
                throw new ArgumentException("Arguments to create token are not valid.");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ExpireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(SecretKey)), SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
