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
    [Route("/api/login")]
    public class LoginController: ControllerBase
    {
        private readonly UsersDAO repository;
        
        public LoginController()
        {
            repository = new UsersDAO();
        }

        [HttpPost]
        public ActionResult<int> ProcessLogin(UserModel user)
        {
            try
            {
                UserModel fetchedUser = repository.GetUserByFullCredentials(user);

                if (fetchedUser == null)
                {
                    // Return login error message
                    throw new Exception("These account details are not valid. Please try again.");
                }
                return fetchedUser.Id;
            } catch(Exception exc)
            {
                return BadRequest(exc.Message);
            }           
        }
    }
}
