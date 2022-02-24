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
        public ActionResult<IEnumerable<PostCommentModel>> Get()
        {
            IEnumerable<PostCommentModel> comments = repo.GetAllComments();
            return comments.ToList();
        }
   
        [Route("/api/posts/{postId}/comments")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<PostCommentModel>> Get(int postId)
        {
            IEnumerable<PostCommentModel> comments = repo.GetCommentsByPostId(postId);
            return comments.OrderByDescending(c => c.Date).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<PostCommentModel> GetOne(int id)
        {
            PostCommentModel comment = repo.GetCommentById(id);
            return comment;
        }

        [HttpPost("[action]")]
        public ActionResult<PostCommentModel> Create([FromBody] PostCommentModel model)
        {
            PostCommentModel comment = repo.Insert(model);
            return comment;
        }

        [HttpPut("[action]")]
        public ActionResult<PostCommentModel> Update([FromBody] PostCommentModel model)
        {
            PostCommentModel comment = repo.Update(model);
            return comment;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<PostCommentModel> Delete(int id)
        {
            PostCommentModel comment = repo.Delete(id);
            return comment;
        }

        [Route("/api/posts/{postId}/comments/delete")]
        [HttpDelete]
        public ActionResult<IEnumerable<PostCommentModel>> DeleteByPostId(int postId)
        {
            IEnumerable<PostCommentModel> comments = repo.DeleteCommentsByPostId(postId);
            return comments.ToList();
        }
    }
}
