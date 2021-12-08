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
   
        [HttpGet("{postId}")]
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
        public ActionResult<int> Create(PostCommentModel comment)
        {
            int id = repository.Insert(comment);
            return id;
        }

        [HttpDelete]
        public ActionResult<int> Delete(int id)
        {
            repository.Delete(id);
            return id;
        }

        [HttpDelete("{postId}")]
        public ActionResult<int> DeleteByPostId(int postId)
        {
            repository.Delete(postId);
            return postId;
        }
    }
}
