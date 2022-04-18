using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechBlog.Models;

namespace TechBlog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]        
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public long Date { get; set; }      

        [Required]
        public string ImgSrc { get; set; }

        [Required]        
        public string Excerpt { get; set; }   

        [Required]
        public string Content { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<PostComment> Comments { get; set; } = new HashSet<PostComment>();
    }
}
