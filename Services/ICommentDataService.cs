using System.Collections.Generic;
using TechBlog.Models;

namespace TechBlog.Services
{
    public interface ICommentDataService
    {
        public IEnumerable<PostComment> GetAllComments();
        public IEnumerable<PostComment> GetCommentsByPostId(int id);
        public PostComment GetCommentById(int id);
        public PostComment Insert(PostComment comment);
        public PostComment Update(PostComment comment);
        public PostComment Delete(int id);
        public IEnumerable<PostComment> DeleteCommentsByPostId(int id);
    }
}
