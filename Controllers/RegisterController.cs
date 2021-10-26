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
            if (repository.IsUsernameFound(user))
            {
                // Return username present error message

            }

            if (repository.IsEmailFound(user))
            {
                // Return email present error message
            }

            int userId = repository.InsertUser(user);
            return userId;
        }
    }
}
