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
        private readonly IPostDataService repo;
        private readonly ICommentDataService commentRepo;

        public PostController(IPostDataService repo, ICommentDataService commentRepo)
        {
            this.repo = repo;
            this.commentRepo = commentRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            List<PostModel> posts = repo.GetAllPosts();            
            return posts;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<PostModel>> AdminGet()
        {
            List<PostModel> posts = repo.GetAllPosts();
            return posts;
        }

        [HttpGet("{id}")]
        public ActionResult<PostModel> GetOne(int id)
        {
            List<PostModel> posts = repo.GetPostById(id);
            return posts[0];            
        }

        [HttpGet("adminGetOne/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<PostModel> AdminGetOne(int id)
        {
            List<PostModel> posts = repo.GetPostById(id);
            return posts[0];
        }
      
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<int> Create([FromBody] PostModel post)
        {
            int postId = repo.Insert(post);            
            return postId;
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<int> Update([FromBody] PostModel post)
        {
            int postId = repo.Update(post);
            return postId;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<int> Delete(int id)
        {
            repo.Delete(id);
            commentRepo.DeleteCommentsByPostId(id);
            return id;
        }
    }
}
