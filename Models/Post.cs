using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechBlog.Models;

namespace TechBlog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public long Date { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Author { get; set; }

        [Required]
        public string ImgSrc { get; set; }

        [Required]        
        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual ICollection<PostComment> Comments { get; set; } = new HashSet<PostComment>();
    }
}
