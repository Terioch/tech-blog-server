using System.ComponentModel.DataAnnotations;

namespace TechBlog.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }

        [Required]
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
    }
}
