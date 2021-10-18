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
                        Author = (string)reader[3],
                        ImgSrc = (string)(reader[4]),
                        Excerpt = (string)reader[5],
                        Content = (string)reader[6],
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

        public int Insert(PostModel post)
        {
            string statement = "INSERT INTO dbo.Posts (title, date, author, imgSrc, excerpt, content) VALUES (@Title, @Date, @Author, @ImgSrc, @Excerpt, @Content)";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Date", post.Date);
            command.Parameters.AddWithValue("@Author", post.Author);
            command.Parameters.AddWithValue("@ImgSrc", post.ImgSrc);
            command.Parameters.AddWithValue("@Excerpt", post.Excerpt);
            command.Parameters.AddWithValue("@Content", post.Content);   

            try
            {
                connection.Open();
                post.Id = Convert.ToInt32(command.ExecuteScalar());
            } catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return post.Id;
        }

        public int Update(PostModel post)
        {
            throw new Exception();
        }

        public int Delete(int Id)
        {
            throw new Exception();
        }
    }
}
