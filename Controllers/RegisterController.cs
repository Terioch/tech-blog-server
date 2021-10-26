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
        public bool ProcessRegistration(UserModel user)
        {
           if (true)
           {
                return false;
           }
        }
    }
}
