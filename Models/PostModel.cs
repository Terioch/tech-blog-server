using System;
using System.Collections.Generic;
using TechBlog.Models;

namespace TechBlog.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long Date { get; set; }
        public string Author { get; set; }
        public string ImgSrc { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public List<PostCommentModel> Comments { get; set; } = new();
    }
}
