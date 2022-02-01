using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechBlog.Models;
using TechBlog.Services;
using System;

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
            List<PostCommentModel> comments = repo.GetAllComments();
            return comments;
        }
   
        [Route("/api/posts/{postId}/comments")]
        [HttpGet]
        public ActionResult<IEnumerable<PostCommentModel>> Get(int postId)
        {
            List<PostCommentModel> comments = repo.GetCommentsByPostId(postId);
            return comments;
        }

        [HttpGet("{id}")]
        public ActionResult<PostCommentModel> GetOne(int id)
        {
            List<PostCommentModel> comments = repo.GetCommentById(id);
            return comments[0];
        }

        [HttpPost("[action]")]
        public ActionResult<int> Create([FromBody] PostCommentModel comment)
        {
            int id = repo.Insert(comment);
            return id;
        }

        [HttpPut("[action]")]
        public ActionResult<int> Update([FromBody] PostCommentModel comment)
        {
            int id = repo.Update(comment);
            return id;
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<int> Delete(int id)
        {
            repo.Delete(id);
            return id;
        }

        [Route("/api/posts/{postId}/comments/delete")]
        [HttpDelete]
        public ActionResult<int> DeleteByPostId(int postId)
        {
            repo.DeleteCommentsByPostId(postId);
            return postId;
        }
    }
}
