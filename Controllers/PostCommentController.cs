using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechBlog.Models;
using TechBlog.Services;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/{postId}/comments")]
    [Authorize(Roles = "Admin, User")]
    public class PostCommentController : ControllerBase
    {
        private ICommentDataService repository;

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

        [HttpGet("{id}")]
        public ActionResult<PostCommentModel> GetOne(int id)
        {
            List<PostCommentModel> comments = repository.GetCommentById(id);
            return comments[0];
        }
    }
}
