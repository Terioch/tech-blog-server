using Microsoft.AspNetCore.Mvc;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TechBlog.ViewModels;

namespace TechBlog.Controllers
{
    [ApiController]  
    [Route("/api/posts")]
    [Authorize(Roles = "Admin")]
    public class PostController : ControllerBase
    {  
        private readonly IPostDataService repo;
        private readonly ICommentDataService commentRepo;
        private readonly ISlugService slugService;
        private readonly ISecurityDataService securityRepo;

        public PostController(IPostDataService repo, ICommentDataService commentRepo, ISlugService slugService, ISecurityDataService securityRepo)
        {
            this.repo = repo;
            this.commentRepo = commentRepo;
            this.slugService = slugService;
            this.securityRepo = securityRepo;
        }        
       
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> Get()
        {
            var posts = repo.GetAllPosts().Take(15);            
            return posts.OrderByDescending(p => p.Date).ToList();
        }

        [HttpGet("[action]")]        
        public ActionResult<IEnumerable<Post>> AdminGet()
        {
            var posts = repo.GetAllPosts();
            return posts.OrderByDescending(p => p.Date).ToList();
        }
        
        [HttpGet("{id}")]        
        [AllowAnonymous]
        public async Task<ActionResult<Post>> GetOne(int id)
        {
            Post post = await repo.GetPostById(id);
            return post;            
        }

        [HttpGet("[action]/{id}")]        
        public async Task<ActionResult<Post>> AdminGetOne(int id)
        {
            Post post = await repo.GetPostById(id);
            return post;
        }

        [HttpGet("search/{searchTerm}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> FilterPostsByTitle(string searchTerm = null)
        {
            var posts = repo.GetAllPosts();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Array.Empty<Post>();
            }

            return posts
                .Where(p => p.Title.ToLowerInvariant().Contains(searchTerm))
                .OrderByDescending(p => p.Date)
                .ToList();
        }        
      
        [HttpPost("[action]")]        
        public ActionResult<Post> Create([FromBody] CreatePostViewModel model)
        {
            Post post = new()
            {
                AuthorId = model.AuthorId,
                Title = model.Title,
                Slug = slugService.Slugify(model.Title),
                Date = model.Date,
                ImgSrc = model.ImgSrc,
                Excerpt = model.Excerpt,
                Content = model.Content
            };            

            // Verify whether title is unique based on slug            
            if (slugService.IsUnique(post.Slug))
            {
                Post newPost = repo.Insert(post);
                return newPost;
            }
            else
            {
                return BadRequest("Cannot create a post with a duplicate title");
            }            
        }

        [HttpPut("[action]")]        
        public async Task<ActionResult<Post>> Update([FromBody] Post model)
        {
            Post post = await repo.GetPostById(model.Id);

            post.AuthorId = model.AuthorId;
            post.Excerpt = model.Excerpt;            
            post.Content = model.Content;
            post.ImgSrc = model.ImgSrc;

            string newSlug = slugService.Slugify(model.Title);

            // Verify whether title is unique based on slug
            if (post.Slug != newSlug)
            {
                if (slugService.IsUnique(newSlug))
                {
                    post.Title = model.Title;
                    post.Slug = newSlug;
                } 
                else
                {
                    return BadRequest("Cannot create a post with a duplicate title");
                }
            }

            repo.Update(post);
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
