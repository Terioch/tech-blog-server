using System.Collections.Generic;
using TechBlog.Models;

namespace TechBlog.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public long Date { get; set; }

        public string ImgSrc { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public UserDTO Author { get; set; }

        public ICollection<PostComment> Comments { get; set; } = new HashSet<PostComment>();
    }
}
