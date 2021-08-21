using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMMPlaH.API.SecurityFilter;
using IMMPlaH.Domain.Enums;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IMMPlaH.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return _userService.GetUsers();
        }

        [HttpGet("{email}")]
        public ActionResult<User> GetUserByEmail(string email)
        {
            return _userService.GetUserByEmail(email);
        }

        [HttpPost]
        public ActionResult<List<string>> LogInUser(Credentials credentials)
        {
            string email = credentials.Email;
            string password = credentials.Password;
            try
            {
                var list = _userService.VerifyPassword(email, password);
                return list;
            }
            catch (UserServiceException e)
            {
                return StatusCode(401, e.Message);
            }
        }

        [HttpPost("type")]
        public ActionResult CheckUserType([FromBody] string token)
        {
            UserTypeEnum result = _userService.CheckUserType(token);
            if (result == UserTypeEnum.Doctor)
                return Ok("doctor");
            if (result == UserTypeEnum.Caregiver)
                return Ok("caregiver");
            if (result == UserTypeEnum.Patient)
                return Ok("patient");
            return Ok("none");
        }
    }
}
