using TechBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Services
{
    public interface IPostDataService
    {
        public IEnumerable<Post> GetAllPosts();
        public Task<Post> GetPostById(int Id);
        public int Insert(Post post);
        public int Update(Post post);
        public int Delete(int Id);
    }
}
