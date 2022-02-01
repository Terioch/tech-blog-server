using TechBlog.Models;
using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TechBlog.Services
{
    public class PostCommentsPgsqlDAO : ICommentDataService
    {
        private readonly string connectionString;

        public PostCommentsPgsqlDAO(IConfiguration config)
        {
            connectionString = config.GetConnectionString("SqlServerDevelopment");
        }

        public List<PostCommentModel> GetAllComments()
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
        }
    }
}
