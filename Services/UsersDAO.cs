using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBlog.Models;
using Microsoft.Extensions.Configuration;

namespace TechBlog.Services
{
    public class UsersDAO
    {
        private readonly string connectionString;
        readonly SecurityHelper security;

        public UsersDAO(IConfiguration config, SecurityHelper security)
        {
            this.security = security;
            connectionString = config.GetConnectionString("SqlServerDevelopment");
        }

        public bool IsUsernameFound(User user)
        {
            string statement = "SELECT username FROM dbo.Users WHERE username = @username";
            return SuccessQuery(user, statement);            
        }

        public bool IsEmailFound(User user)
        {
            string statement = "SELECT email FROM dbo.Users WHERE email = @email";
            return SuccessQuery(user, statement);
        }        

        public User GetUserByFullCredentials(User user)
        {
            string statement = "SELECT * FROM dbo.Users WHERE username = @username AND email = @email AND password = @password";
            string salt = FetchSaltQuery(user);

            if (salt == null) return null;            
            user.Password = security.HashPassword(user.Password, salt);
            User fetchedUser = FetchUserQuery(user, statement);
            return fetchedUser;
        }        

        public int InsertUser(User user)
        {
            string statement = "INSERT INTO dbo.Users (username, email, password, salt) OUTPUT Inserted.Id VALUES (@username, @email, @password, @salt)";
            string salt = security.GenerateSalt();
            user.Password = security.HashPassword(user.Password, salt);
            int id = InsertQuery(user, statement, salt);                       
            return id;
        }
        
        public void InsertUserRole(int id, string roleName)
        {
            string statement = "SELECT * FROM dbo.Roles WHERE name = @name";
            Role role = FetchRoleByNameQuery(roleName, statement);
            InsertUserRoleQuery(id, role.Id);
        }

        public string GetRoleByUserId(int id)
        {            
            string statement = "SELECT roleId FROM dbo.UserRoles WHERE userId = @id";
            int roleId = FetchIdQuery(id, statement);
            statement = "SELECT * from dbo.Roles WHERE id = @id";
            Role role = FetchRoleByIdQuery(roleId, statement);
            return role.Name;
        }        

        public int InsertQuery(User user, string statement, string salt)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);            
            int id = -1;

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@salt", salt);

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

        public void InsertUserRoleQuery(int userId, int roleId)
        {
            string statement = "INSERT into dbo.UserRoles VALUES (@userId, @roleId)";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@roleId", roleId);

            try
            {
                connection.Open();
                command.ExecuteScalar();               
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public bool SuccessQuery(User user, string statement)
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


        public User FetchUserQuery(User user, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            User fetchedUser = null;

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    fetchedUser = new User
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

        public int FetchIdQuery(int id, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            command.Parameters.AddWithValue("@id", id);
            int newId = -1;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    newId = (int)reader[0];
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return newId;
        }

        public Role FetchRoleByIdQuery(int roleId, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            command.Parameters.AddWithValue("@id", roleId);
            Role role = new();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    role = new()
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1]
                    };
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return role;
        }

        public Role FetchRoleByNameQuery(string roleName, string statement)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            command.Parameters.AddWithValue("@name", roleName);
            Role role = new();

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    role = new() 
                    { 
                        Id = (int)reader[0],
                        Name = (string)reader[1]
                    };
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return role;
        }

        public string FetchSaltQuery(User user)
        {
            string statement = "SELECT salt FROM dbo.Users WHERE username = @username";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(statement, connection);
            string salt = null;

            command.Parameters.AddWithValue("@username", user.Username);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {        
                    salt = (string)reader[0];
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            return salt;
        }
    }
}
