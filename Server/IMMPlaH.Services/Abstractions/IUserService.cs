using System;
using System.Collections.Generic;
using System.Text;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Domain.Models;

namespace IMMPlaH.Services.Abstractions
{
    public interface IUserService
    {
        List<User> GetUsers();

        User GetUserByEmail(string email);

        List<string> VerifyPassword(string email, string password);

        User GetUserById(int id);

        UserTypeEnum CheckUserType(string token);
    }
}
