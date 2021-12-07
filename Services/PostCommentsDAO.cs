using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TechBlog.Models;

namespace TechBlog.Services
{
    public class PostCommentsDAO : ICommentDataService
    {
        readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TechBlog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<PostCommentModel> GetAllComments()
        {
            string statement = "SELECT * FROM dbo.PostComments";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            List<PostCommentModel> comments = new();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(new()
                    {
                        Id = (int)reader[0],
                        PostId = (int)reader[1],
                        Value = (string)reader[2],
                        SenderUsername = (string)reader[3],
                        Date = (DateTime)reader[4],
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
            string statement = "SELECT * FROM dbo.PostComments WHERE id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            List<PostCommentModel> comments = new();

            command.Parameters.AddWithValue("@Id", Id);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(new()
                    {
                        Id = (int)reader[0],
                        PostId = (int)reader[1],
                        Value = (string)reader[2],
                        SenderUsername= (string)reader[3],
                        Date = (DateTime)reader[4],
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
            string statement = "INSERT INTO dbo.PostComments (PostId, Value, SenderUsername, Date) OUTPUT Inserted.Id VALUES (@PostId, @Value, @SenderUsername, @Date)";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("@PostId", comment.PostId);
            command.Parameters.AddWithValue("@Value", comment.Value);
            command.Parameters.AddWithValue("@SenderUsername", comment.SenderUsername);
            command.Parameters.AddWithValue("@Date", comment.Date);

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
            string statement = "UPDATE dbo.PostComments SET Value = @Value, Date = @Date OUTPUT Inserted.Id WHERE id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("@Id", comment.Id);
            command.Parameters.AddWithValue("@Value", comment.Value);
            command.Parameters.AddWithValue("@Date", comment.Date);           

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
            string statement = "DELETE FROM dbo.PostComments OUTPUT Inserted.Id WHERE Id = @Id";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("@Id", id);

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
    }
}
