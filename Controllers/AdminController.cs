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
    [Authorize]
    public class AdminController : ControllerBase
    {
        readonly PostsDAO repository;

        public AdminController()
        {
            repository = new PostsDAO();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            List<PostModel> posts = repository.GetAllPosts();
            return posts;
        }
    }
}
