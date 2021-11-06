using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechBlog.Models;
using TechBlog.Services;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {        
        [HttpPost]
        public ActionResult<string> IsAdmin()
        {
            return "User is an admin";
        }
    }
}
