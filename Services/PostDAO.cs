using TechBlog.Models;
using TechBlog.Utility;
using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TechBlog.Contexts;
using System.Threading.Tasks;
using TechBlog.Extensions;

namespace TechBlog.Services
{
    public class PostDAO : IPostDataService
    {
        private readonly TechBlogDbContext context;

        public PostDAO(TechBlogDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return context.Posts.Include(p => p.Author);
        }              

        public async Task<Post> GetPostById(int id)
        {
            return await context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> GetPostBySlug(string slug)
        {
            return await context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public int Insert(Post post)
        {
            context.Posts.Add(post);
            return context.SaveChanges();            
        }

        public int Update(Post post)
        {
            EntityEntry<Post> attachedPost = context.Posts.Attach(post);
            attachedPost.State = EntityState.Modified;
            return context.SaveChanges();
        }

        public int Delete(int id)
        {
            Post post = context.Posts.Find(id);
            context.Posts.Remove(post);
            return context.SaveChanges();
        }

        /*public List<PostModel> GetAllPosts()
        {
            string statement = "SELECT * FROM dbo.posts";
            NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new();
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = statement;
            List<PostModel> posts = new();

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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
            return posts;
        }

        public List<PostModel> GetPostById(int Id)
        {
            string statement = "SELECT * FROM dbo.Posts WHERE id = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            List<PostModel> posts = new();

            command.Parameters.AddWithValue("$1", Id);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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
            return posts;
        }

        public int Insert(PostModel post)
        {
            string statement = "INSERT INTO dbo.Posts (title, date, author, imgSrc, excerpt, content) OUTPUT Inserted.Id VALUES ($1, $2, $3, $4, $5, $6)";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("$1", post.Title);
            command.Parameters.AddWithValue("$2", post.Date);
            command.Parameters.AddWithValue("$3", post.Author);
            command.Parameters.AddWithValue("$4", post.ImgSrc);
            command.Parameters.AddWithValue("$5", post.Excerpt);
            command.Parameters.AddWithValue("$6", post.Content);

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

        public int Update(PostModel post)
        {
            string statement = "UPDATE dbo.Posts SET title = $1, date = $2, author = $3, imgSrc = $4, excerpt = $5, content = $6 OUTPUT Inserted.Id WHERE id = $7";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            int id = -1;
            
            command.Parameters.AddWithValue("$1", post.Title);
            command.Parameters.AddWithValue("$2", post.Date);
            command.Parameters.AddWithValue("$3", post.Author);
            command.Parameters.AddWithValue("$4", post.ImgSrc);
            command.Parameters.AddWithValue("$5", post.Excerpt);
            command.Parameters.AddWithValue("$6", post.Content);
            command.Parameters.AddWithValue("$7", post.Id);

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
            string statement = "DELETE FROM dbo.Posts WHERE Id = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("$1", id);

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
        }*/
    }
}
