using Microsoft.AspNetCore.Mvc;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/posts")]
    public class PostController : ControllerBase
    {      
        readonly PostsDAO repository;

        public PostController()
        {          
            repository = new PostsDAO();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            List<PostModel> posts = repository.GetAllPosts();            
            return posts;
        }

        [HttpGet("adminGet")]
        [Authorize]
        public ActionResult<IEnumerable<PostModel>> AdminGet()
        {
            List<PostModel> posts = repository.GetAllPosts();
            return posts;
        }

        [HttpGet("{id}")]
        public ActionResult<PostModel> GetOne(int id)
        {
            List<PostModel> posts = repository.GetPostById(id);
            return posts[0];            
        }

        [HttpGet("adminGetOne/{id}")]
        [Authorize]
        public ActionResult<PostModel> AdminGetOne(int id)
        {
            List<PostModel> posts = repository.GetPostById(id);
            return posts[0];
        }
      
        [HttpPost("create")]
        [Authorize]
        public ActionResult<int> Create([FromBody] PostModel post)
        {
            int postId = repository.Insert(post);            
            return postId;
        }

        [HttpPut("update")]
        [Authorize]
        public ActionResult<int> Update([FromBody] PostModel post)
        {
            int postId = repository.Update(post);
            return postId;
        }
    }
}
