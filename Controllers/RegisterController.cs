using TechBlog.Models;
using TechBlog.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/register")]
    public class RegisterController: ControllerBase
    {
        readonly UsersDAO repository;    

        public RegisterController()
        {
            repository = new UsersDAO();
        }

        [HttpPost]
        public ActionResult<int> ProcessRegistration(UserModel user)            
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
                return userId;
            } catch(Exception exc)
            {
                return BadRequest(exc.Message);
            }            
        }
    }
}
