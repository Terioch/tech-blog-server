using Microsoft.EntityFrameworkCore;
using TechBlog.Models;

namespace TechBlog.Contexts
{
    public class TechBlogDbContext : DbContext
    {
        public TechBlogDbContext(DbContextOptions<TechBlogDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
