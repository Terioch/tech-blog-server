using TechBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Services
{
    interface IPostDataService
    {
        public List<PostModel> GetAllPosts();
        public List<PostModel> GetPostById(int Id);
        public PostModel Create(PostModel post);
        public int Delete(int Id);
    }
}
