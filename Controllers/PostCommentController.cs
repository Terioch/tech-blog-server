using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechBlog.Models;
using TechBlog.Services;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/comments")]
    [Authorize(Roles = "Admin, User")]
    public class PostCommentController : ControllerBase
    {
        private readonly ICommentDataService repository;

        public PostCommentController(ICommentDataService repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostCommentModel>> Get()
        {
            List<PostCommentModel> comments = repository.GetAllComments();
            return comments;
        }
   
        [Route("/api/posts/{postId}/comments")]
        [HttpGet]
        public ActionResult<IEnumerable<PostCommentModel>> Get(int postId)
        {
            List<PostCommentModel> comments = repository.GetCommentsByPostId(postId);
            return comments;
        }

        [HttpGet("{id}")]
        public ActionResult<PostCommentModel> GetOne(int id)
        {
            List<PostCommentModel> comments = repository.GetCommentById(id);
            return comments[0];
        }

        [HttpPost("create")]
        public ActionResult<int> Create([FromBody] PostCommentModel comment)
        {
            int id = repository.Insert(comment);
            return id;
        }

        [HttpPut("update")]
        public ActionResult<int> Update([FromBody] PostCommentModel comment)
        {
            int id = repository.Update(comment);
            return id;
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<int> Delete(int id)
        {
            repository.Delete(id);
            return id;
        }

        [Route("/api/posts/{postId}/comments")]
        [HttpDelete]
        public ActionResult<int> DeleteByPostId(int postId)
        {
            repository.DeleteCommentsByPostId(postId);
            return postId;
        }
    }
}
