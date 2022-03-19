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
        public Post GetPostById(int Id);
        public Post Insert(Post post);
        public Post Update(Post post);
        public Post Delete(int Id);
    }
}
