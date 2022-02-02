using TechBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Services
{
    public interface IPostDataService
    {
        public IEnumerable<PostModel> GetAllPosts();
        public PostModel GetPostById(int Id);
        public PostModel Insert(PostModel post);
        public PostModel Update(PostModel post);
        public PostModel Delete(int Id);
    }
}
