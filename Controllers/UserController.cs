using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TechBlog.Models;
using TechBlog.Services;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public ActionResult<string> IsAuthenticated()
        {
            return "User is authenticated";
        }

        readonly UsersDAO repository;
        readonly SecurityService security;

        public UserController()
        {
            repository = new UsersDAO();
            security = new SecurityService();
        }

        [HttpPost("register")]
        public ActionResult<int> ProcessRegistration([FromBody] UserModel user)
        {
            try
            {
                if (repository.IsUsernameFound(user))
                {
                    // Return username present error message
                    throw new Exception("This username is taken. Please choose a different name.");
                }

                if (repository.IsEmailFound(user))
                {
                    // Return email present error message                
                    throw new Exception("This email address is already associated with an account. Either use a different email or login if you are the account holder.");
                }

                int userId = repository.InsertUser(user);
                string token = security.GenerateToken(user);
                return Ok(new { Id = userId, Token = token });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return Unauthorized(exc.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult<int> ProcessLogin([FromBody] UserModel user)
        {
            try
            {
                UserModel fetchedUser = repository.GetUserByFullCredentials(user);

                if (fetchedUser == null)
                {
                    // Return login error message
                    throw new Exception("These account details are not valid. Please try again.");
                }
                string token = security.GenerateToken(user);
                return Ok(new { fetchedUser.Id, Token = token });
            }
            catch (Exception exc)
            {
                return Unauthorized(exc.Message);
            }
        }
    }
}
