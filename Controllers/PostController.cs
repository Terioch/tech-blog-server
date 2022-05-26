using Microsoft.AspNetCore.Mvc;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TechBlog.ViewModels;
using TechBlog.DTOs;
using TechBlog.Extensions;

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
        public ActionResult<IEnumerable<PostDTO>> Get()
        {
            var posts = repo.GetAllPosts().Take(15);            
            return posts.OrderByDescending(p => p.Date).Select(p => p.AsDTO()).ToList();
        }

        [HttpGet("[action]")]        
        public ActionResult<IEnumerable<PostDTO>> AdminGet()
        {
            var posts = repo.GetAllPosts();
            return posts.OrderByDescending(p => p.Date).Select(p => p.AsDTO()).ToList();
        }
        
        [HttpGet("{id}")]        
        [AllowAnonymous]
        public async Task<ActionResult<PostDTO>> GetOne(int id)
        {
            Post post = await repo.GetPostById(id);          
            return post.AsDTO();            
        }

        [HttpGet("[action]/{id}")]        
        public async Task<ActionResult<PostDTO>> AdminGetOne(int id)
        {
            Post post = await repo.GetPostById(id);
            return post.AsDTO();
        }

        [HttpGet("search/{searchTerm}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDTO>> FilterPostsByTitle(string searchTerm = null)
        {
            var posts = repo.GetAllPosts();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Array.Empty<PostDTO>();
            }

            return posts
                .Where(p => p.Title.ToLowerInvariant().Contains(searchTerm))
                .OrderByDescending(p => p.Date)
                .Select(p => p.AsDTO())
                .ToList();
        }        
      
        [HttpPost("[action]")]        
        public ActionResult<PostDTO> Create([FromBody] CreatePostViewModel model)
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
                repo.Insert(post);
                return post.AsDTO();
            }
            else
            {
                return BadRequest("Cannot update a post with a duplicate title");
            }            
        }

        [HttpPut("[action]")]        
        public async Task<ActionResult<PostDTO>> Update([FromBody] EditPostViewModel model)
        {
            Post post = await repo.GetPostById(model.Id);

            post.AuthorId = model.AuthorId;
            post.Date = model.Date;
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
            Post updatedPost = await repo.GetPostById(post.Id);
            return updatedPost.AsDTO();
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<int> Delete(int id)
        {
            repo.Delete(id);
            commentRepo.DeleteCommentsByPostId(id);
            return id;
        }
    }
}
