using Microsoft.EntityFrameworkCore;
using TechBlog.Models;

namespace TechBlog.Contexts
{
    public class TechBlogDbContext : DbContext
    {
        public TechBlogDbContext(DbContextOptions<TechBlogDbContext> options) : base(options)
        {
        }

        public DbSet<RoleModel> Roles { get; set; }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserRoleModel> UserRoles { get; set; }

        public DbSet<PostModel> Posts { get; set; }

        public DbSet<PostCommentModel> PostComments { get; set; }
    }
}
