using TechBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/register")]
    public class LoginController: ControllerBase
    {
        [HttpPost]
        public bool ProcessLogin(UserModel user)
        {
            return false;
        }
    }
}
