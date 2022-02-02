using TechBlog.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TechBlog.Services
{
    public class PostSqlDAO
    {
        private readonly string connectionString;

        public PostSqlDAO(IConfiguration config)
        {
            connectionString = config.GetConnectionString("SqlServerDevelopment");
        }

        public IEnumerable<PostModel> GetAllPosts()
        {
            string statement = "SELECT * FROM dbo.Posts";
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
                        Date = (long)reader[2],
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

        public PostModel GetPostById(int Id)
        {
            string statement = "SELECT * FROM dbo.Posts WHERE id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            List<PostModel> posts = new();

            command.Parameters.AddWithValue("@Id", Id);

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
                        Date = (long)reader[2],
                        Author = (string)reader[3],
                        ImgSrc = (string)(reader[4]),
                        Excerpt = (string)reader[5],
                        Content = (string)reader[6],
                    });
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return posts[0];
        }

        public int Insert(PostModel post)
        {
            string statement = "INSERT INTO dbo.Posts (title, date, author, imgSrc, excerpt, content) OUTPUT Inserted.Id VALUES (@Title, @Date, @Author, @ImgSrc, @Excerpt, @Content)";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Date", post.Date);
            command.Parameters.AddWithValue("@Author", post.Author);
            command.Parameters.AddWithValue("@ImgSrc", post.ImgSrc);
            command.Parameters.AddWithValue("@Excerpt", post.Excerpt);
            command.Parameters.AddWithValue("@Content", post.Content);   

            try
            {
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            } catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return id;
        }

        public int Update(PostModel post)
        {
            string statement = "UPDATE dbo.Posts SET title = @Title, date = @Date, author = @Author, imgSrc = @ImgSrc, excerpt = @Excerpt, content = @Content OUTPUT Inserted.Id WHERE id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            int id = -1;
            
            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Date", post.Date);
            command.Parameters.AddWithValue("@Author", post.Author);
            command.Parameters.AddWithValue("@ImgSrc", post.ImgSrc);
            command.Parameters.AddWithValue("@Excerpt", post.Excerpt);
            command.Parameters.AddWithValue("@Content", post.Content);
            command.Parameters.AddWithValue("@Id", post.Id);

            try
            {
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return id;
        }

        public int Delete(int id)
        {
            string statement = "DELETE FROM dbo.Posts WHERE Id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("@Id", id);

            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return id;
        }
    }
}
