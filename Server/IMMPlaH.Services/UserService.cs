using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IMMPlaH.DataAccess;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;

namespace IMMPlaH.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<User> GetUsers()
        {
            return _dbContext.User.ToList();
        }

        public User GetUserById(int id)
        {
            return _dbContext.User.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            var user = _dbContext.User.FirstOrDefault(a => a.Email == email);

            return user;
        }

        public List<string> VerifyPassword(string email, string password)
        {
            var user = GetUserByEmail(email);

            if (user == null)
            {
                throw new UserServiceException("User with this email does not exist."); //not registered
            }

            if (user.Password != password)
            {
                throw new UserServiceException("The password for this user is incorrect."); //wrong password
            }

            AuthentificationService authService = new AuthentificationService();

            Claim[] Claims = new Claim[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            string token = authService.GenerateToken(Claims);

            if (!authService.IsTokenValid(token))
                throw new UnauthorizedAccessException();

            return new List<string> { token, user.Type };
        }

        public UserTypeEnum CheckUserType(string token)
        {
            if (token == "null" || token == "" || token == null)
            {
                return UserTypeEnum.None;
            }
            else
            {
                var tokenObject = new JwtSecurityToken(token);
                var userId = tokenObject.Payload[ClaimTypes.Sid].ToString();
                if (userId == null)
                {
                    return UserTypeEnum.None;
                }
                else if (userId == "1")
                {
                    return UserTypeEnum.Doctor;
                }
                else if (userId == "2")
                {
                    return UserTypeEnum.Caregiver;
                }
                else if (userId == "3")
                {
                    return UserTypeEnum.Patient;
                }
            }
            return UserTypeEnum.None;
        }
    }
}
