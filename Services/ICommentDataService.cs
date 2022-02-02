using System.Collections.Generic;
using TechBlog.Models;

namespace TechBlog.Services
{
    public interface ICommentDataService
    {
        public IEnumerable<PostCommentModel> GetAllComments();
        public IEnumerable<PostCommentModel> GetCommentsByPostId(int id);
        public PostCommentModel GetCommentById(int id);
        public PostCommentModel Insert(PostCommentModel comment);
        public PostCommentModel Update(PostCommentModel comment);
        public PostCommentModel Delete(int id);
        public IEnumerable<PostCommentModel> DeleteCommentsByPostId(int id);
    }
}
