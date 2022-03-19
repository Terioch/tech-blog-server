using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Linq;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/comments")]
    [Authorize]
    public class PostCommentController : ControllerBase
    {
        private readonly ICommentDataService repo;

        public PostCommentController(ICommentDataService repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostComment>> Get()
        {
            IEnumerable<PostComment> comments = repo.GetAllComments();
            return comments.ToList();
        }
   
        [Route("/api/posts/{postId}/comments")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<PostComment>> Get(int postId)
        {
            IEnumerable<PostComment> comments = repo.GetCommentsByPostId(postId);
            return comments.OrderByDescending(c => c.Date).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<PostComment> GetOne(int id)
        {
            PostComment comment = repo.GetCommentById(id);
            return comment;
        }

        [HttpPost("[action]")]
        public ActionResult<PostComment> Create([FromBody] PostComment model)
        {
            PostComment comment = repo.Insert(model);
            return comment;
        }

        [HttpPut("[action]")]
        public ActionResult<PostComment> Update([FromBody] PostComment model)
        {
            PostComment comment = repo.Update(model);
            return comment;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<PostComment> Delete(int id)
        {
            PostComment comment = repo.Delete(id);
            return comment;
        }

        [Route("/api/posts/{postId}/comments/delete")]
        [HttpDelete]
        public ActionResult<IEnumerable<PostComment>> DeleteByPostId(int postId)
        {
            IEnumerable<PostComment> comments = repo.DeleteCommentsByPostId(postId);
            return comments.ToList();
        }
    }
}
