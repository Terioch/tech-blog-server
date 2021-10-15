using TechBlog.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Services
{
    public class PostsDAO: IPostDataService
    {
        readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TechBlog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<PostModel> GetAllPosts()
        {
            string statement = "SELECT * FROM dbo.Posts WHERE id < 60";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            List<PostModel> posts = new();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    posts.Add(new PostModel
                    {
                        Id = (int)reader[0],
                        Title = (string)reader[1],
                        Date = (DateTime)reader[2],
                        Excerpt = (string)reader[3],
                    });
                }
            } catch(Exception exc)
            {                
                throw new Exception(exc.Message);
            }
            return posts;
        }

        public List<PostModel> GetPostById(int Id)
        {
            throw new Exception();
        }

        public PostModel Create(PostModel post)
        {
            throw new Exception();
        }

        public int Delete(int Id)
        {
            throw new Exception();
        }
    }
}
