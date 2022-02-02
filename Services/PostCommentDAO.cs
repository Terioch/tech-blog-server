using TechBlog.Models;
using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TechBlog.Utility;
using TechBlog.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace TechBlog.Services
{
    public class PostCommentDAO : ICommentDataService
    {
        private readonly TechBlogDbContext context;

        public PostCommentDAO(TechBlogDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<PostCommentModel> GetAllComments()
        {
            return context.PostComments;
        }

        public PostCommentModel GetCommentById(int id)
        {
            return context.PostComments.Find(id);
        }

        public IEnumerable<PostCommentModel> GetCommentsByPostId(int id)
        {
            return context.PostComments.Where(c => c.PostId == id);
        }

        public PostCommentModel Insert(PostCommentModel comment)
        {
            context.PostComments.Add(comment);
            context.SaveChanges();
            return comment;
        }

        public PostCommentModel Update(PostCommentModel comment)
        {
            EntityEntry<PostCommentModel> attachedComment = context.PostComments.Attach(comment);
            attachedComment.State = EntityState.Modified;
            context.SaveChanges();
            return comment;
        }

        public PostCommentModel Delete(int id)
        {
            PostCommentModel comment = context.PostComments.Find(id);
            return comment;
        }

        public IEnumerable<PostCommentModel> DeleteCommentsByPostId(int id)
        {
            var comments = context.PostComments.Where(c => c.PostId == id);
            context.PostComments.RemoveRange(comments);
            return comments;
        }        

        /*public List<PostCommentModel> GetAllComments()
        {
            string statement = "SELECT * FROM dbo.PostComments";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            List<PostCommentModel> comments = new();

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(new()
                    {
                        Id = (int)reader[0],
                        PostId = (int)reader[1],
                        Value = (string)reader[2],
                        SenderUsername = (string)reader[3],
                        Date = (long)reader[4],
                    });
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return comments;
        }

        public List<PostCommentModel> GetCommentsByPostId(int postId)
        {
            string statement = "SELECT * FROM dbo.PostComments WHERE PostId = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            List<PostCommentModel> comments = new();

            command.Parameters.AddWithValue("$1", postId);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(new()
                    {
                        Id = (int)reader[0],
                        PostId = (int)reader[1],
                        Value = (string)reader[2],
                        SenderUsername = (string)reader[3],
                        Date = (long)reader[4],
                    });
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return comments;
        }

        public List<PostCommentModel> GetCommentById(int Id)
        {
            string statement = "SELECT * FROM dbo.PostComments WHERE id = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            List<PostCommentModel> comments = new();

            command.Parameters.AddWithValue("$1", Id);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(new()
                    {
                        Id = (int)reader[0],
                        PostId = (int)reader[1],
                        Value = (string)reader[2],
                        SenderUsername = (string)reader[3],
                        Date = (long)reader[4],
                    });
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return comments;
        }

        public int Insert(PostCommentModel comment)
        {
            string statement = "INSERT INTO dbo.PostComments (PostId, Value, SenderUsername, Date) OUTPUT Inserted.Id VALUES ($1, $2, $3, $4)";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("$1", comment.PostId);
            command.Parameters.AddWithValue("$2", comment.Value);
            command.Parameters.AddWithValue("$3", comment.SenderUsername);
            command.Parameters.AddWithValue("$4", comment.Date);

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

        public int Update(PostCommentModel comment)
        {
            string statement = "UPDATE dbo.PostComments SET Value = $1, Date = $2 OUTPUT Inserted.Id WHERE id = $3";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            int id = -1;
            
            command.Parameters.AddWithValue("$1", comment.Value);
            command.Parameters.AddWithValue("$2", comment.Date);
            command.Parameters.AddWithValue("$3", comment.Id);

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
            string statement = "DELETE FROM dbo.PostComments WHERE Id = $1";
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
        }

        public int DeleteCommentsByPostId(int postId)
        {
            string statement = "DELETE FROM dbo.PostComments WHERE PostId = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("$1", postId);

            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            return postId;
        }*/
    }
}
