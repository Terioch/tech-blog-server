﻿using Microsoft.AspNetCore.Mvc;
using TechBlog.Models;
using TechBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/posts")]
    public class PostController : ControllerBase
    {
        readonly PostsDAO repository;

        public PostController()
        {
            repository = new PostsDAO();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PostModel>> Get()
        {
            // TODO: Convert post title to slug via post DTO
            List<PostModel> posts = repository.GetAllPosts();
            return posts;
        }

        [HttpGet("{id}")]
        public ActionResult<PostModel> GetOne(int id)
        {
            List<PostModel> posts = repository.GetPostById(id);
            return posts[0];            
        }

        [HttpPost("create")]
        public ActionResult<PostModel> Create([FromBody] PostModel post)
        {
            int postId = repository.Insert(post);
            // TODO: Retrieve post with correct database Id. Current post is copied and loses reference once passed into Insert method
            post.Id = postId;
            return post;
        }
    }
}
