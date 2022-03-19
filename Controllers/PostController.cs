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
    [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> Get()
        {
            IEnumerable<Post> posts = repo.GetAllPosts();            
            return posts.OrderByDescending(p => p.Date).ToList();
        }

        [HttpGet("[action]")]        
        public ActionResult<IEnumerable<Post>> AdminGet()
        {
            IEnumerable<Post> posts = repo.GetAllPosts();
            return posts.OrderByDescending(p => p.Date).ToList();
        }
        
        [HttpGet("{id}")]        
        [AllowAnonymous]
        public ActionResult<Post> GetOne(int id)
        {
            Post post = repo.GetPostById(id);
            return post;            
        }

        [HttpGet("adminGetOne/{id}")]        
        public ActionResult<Post> AdminGetOne(int id)
        {
            Post post = repo.GetPostById(id);
            return post;
        }
      
        [HttpPost("[action]")]        
        public ActionResult<Post> Create([FromBody] Post model)
        {
            Post post = repo.Insert(model);            
            return post;
        }

        [HttpPut("[action]")]        
        public ActionResult<Post> Update([FromBody] Post model)
        {
            Post post = repo.Update(model);
            return post;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<Post> Delete(int id)
        {
            Post post = repo.Delete(id);
            commentRepo.DeleteCommentsByPostId(id);
            return post;
        }
    }
}
