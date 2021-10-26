using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TechBlog.Models;

namespace TechBlog.Services
{
    public class UsersDAO
    {
        readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TechBlog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool IsUsernameFound(UserModel user)
        {
            string statement = "SELECT username FROM dbo.Users WHERE username = @username";
            return SuccessQuery(user, statement);            
        }

        public bool IsEmailFound(UserModel user)
        {
            string statement = "SELECT email FROM dbo.Users WHERE email = @email";
            return SuccessQuery(user, statement);
        }        

        public UserModel GetUserByUsernameAndEmail(UserModel user)
        {
            string statement = "SELECT * FROM dbo.Users WHERE username = @username AND email = @email";
            UserModel fetchedUser = FetchQuery(user, statement);
            return fetchedUser;
        }

        public int InsertUser(UserModel user)
        {
            string statement = "INSERT INTO dbo.Users (username, email, password) OUTPUT Inserted.Id VALUES (@username, @email, @password)";
            int id = InsertQuery(user, statement);
            return id;
        }

        public int InsertQuery(UserModel user, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            string hashedPassword = user.Password; // TODO: Hash password
            int id = -1;

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", hashedPassword);

            try
            {
                connection.Open();
                id = (int)command.ExecuteScalar();               
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return id;
        }

        public bool SuccessQuery(UserModel user, string statement)
        {
            bool success = false;
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    success = true;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return success;
        }

        public UserModel FetchQuery(UserModel user, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            UserModel fetchedUser = null;

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);  

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    fetchedUser = new UserModel
                    {
                        Id = (int)reader[0],
                        Username = (string)reader[1],
                        Email = (string)reader[2],
                        Password = (string)reader[3],
                    };
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return fetchedUser;
        }       
    }
}
