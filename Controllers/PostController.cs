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
            IEnumerable<PostModel> posts = repo.GetAllPosts();            
            return posts.OrderByDescending(p => p.Date).ToList();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<PostModel>> AdminGet()
        {
            IEnumerable<PostModel> posts = repo.GetAllPosts();
            return posts.OrderByDescending(p => p.Date).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<PostModel> GetOne(int id)
        {
            PostModel post = repo.GetPostById(id);
            return post;            
        }

        [HttpGet("adminGetOne/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<PostModel> AdminGetOne(int id)
        {
            PostModel post = repo.GetPostById(id);
            return post;
        }
      
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<PostModel> Create([FromBody] PostModel model)
        {
            PostModel post = repo.Insert(model);            
            return post;
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<PostModel> Update([FromBody] PostModel model)
        {
            PostModel post = repo.Update(model);
            return post;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<PostModel> Delete(int id)
        {
            PostModel post = repo.Delete(id);
            commentRepo.DeleteCommentsByPostId(id);
            return post;
        }
    }
}
