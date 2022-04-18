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
        private readonly ISlugService slugService;
        private readonly ISecurityDataService securityRepo;

        public PostController(IPostDataService repo, ICommentDataService commentRepo, ISlugService slugService, ISecurityDataService securityRepo)
        {
            this.repo = repo;
            this.commentRepo = commentRepo;
            this.slugService = slugService;
            this.securityRepo = securityRepo;
        }

        /*[HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> RunTempUpdates()
        {
            var posts = repo.GetAllPosts().ToList();
            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].AuthorId = securityRepo.GetUserByUsername(posts[i].Author).Id;
                posts[i].Slug = slugService.Slugify(posts[i].Title);
                repo.Update(posts[i]);
            }
            return posts.ToList();
        }*/
       
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
        public ActionResult<Post> GetOne(int id)
        {
            Post post = repo.GetPostById(id);
            return post;            
        }

        [HttpGet("[action]/{id}")]        
        public ActionResult<Post> AdminGetOne(int id)
        {
            Post post = repo.GetPostById(id);
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
        public ActionResult<Post> Create([FromBody] Post model)
        {
            model.Slug = slugService.Slugify(model.Title);
            Post post = repo.Insert(model);               
            return post;
        }

        [HttpPut("[action]")]        
        public ActionResult<Post> Update([FromBody] Post model)
        {
            Post post = repo.GetPostById(model.Id);
             
            post.Excerpt = model.Excerpt;
            // post.AuthorId = model.AuthorId;
            post.Author = model.Author;
            post.Content = model.Content;
            post.ImgSrc = model.ImgSrc;

            string newSlug = slugService.Slugify(post.Title);

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
