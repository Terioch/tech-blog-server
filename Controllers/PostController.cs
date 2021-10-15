using Microsoft.AspNetCore.Mvc;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api")]
    public class PostController : Controller
    {
        readonly PostsDAO repository;
        public PostController()
        {
            repository = new PostsDAO();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> GetAll()
        {
            List<PostModel> posts = repository.GetAllPosts();
            return posts;
        }
    }
}
