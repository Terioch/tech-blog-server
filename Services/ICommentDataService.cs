using System.Collections.Generic;
using TechBlog.Models;

namespace TechBlog.Services
{
    public interface ICommentDataService
    {
        public List<PostCommentModel> GetAllComments();
        public List<PostCommentModel> GetCommentById(int id);
        public int Insert(PostCommentModel comment);
        public int Update(PostCommentModel comment);
        public int Delete(int id);
    }
}
