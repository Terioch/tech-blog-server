using TechBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Services
{
    public interface IPostDataService
    {
        public List<PostModel> GetAllPosts();
        public List<PostModel> GetPostById(int Id);
        public int Insert(PostModel post);
        public int Update(PostModel post);
        public int Delete(int Id);
    }
}
