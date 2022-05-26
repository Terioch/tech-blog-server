using TechBlog.DTOs;
using TechBlog.Models;

namespace TechBlog.Extensions
{
    public static class DTOExtensions
    {
        public static UserDTO AsDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public static PostDTO AsDTO(this Post post)
        {
            return new PostDTO
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Title = post.Title,
                Slug = post.Slug,
                Date = post.Date,
                ImgSrc = post.ImgSrc,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Author = post.Author.AsDTO(),
                Comments = post.Comments,
            };
        }
    }
}
