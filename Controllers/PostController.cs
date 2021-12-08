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
        private readonly ICommentDataService commentRepository;

        public PostController(ICommentDataService commentRepository)
        {          
            repository = new PostsDAO();
            this.commentRepository = commentRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            List<PostModel> posts = repository.GetAllPosts();            
            return posts;
        }

        [HttpGet("adminGet")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult<PostModel> AdminGetOne(int id)
        {
            List<PostModel> posts = repository.GetPostById(id);
            return posts[0];
        }
      
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public ActionResult<int> Create([FromBody] PostModel post)
        {
            int postId = repository.Insert(post);            
            return postId;
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult<int> Update([FromBody] PostModel post)
        {
            int postId = repository.Update(post);
            return postId;
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<int> Delete(int id)
        {
            repository.Delete(id);
            commentRepository.DeleteCommentsByPostId(id);
            return id;
        }
    }
}
