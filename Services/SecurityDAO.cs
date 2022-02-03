using TechBlog.Models;
using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using TechBlog.Contexts;
using System.Linq;

namespace TechBlog.Services
{
    public class SecurityDAO : ISecurityDataService
    {
        private readonly TechBlogDbContext context;
        private readonly SecurityHelper security;

        public SecurityDAO(TechBlogDbContext context, SecurityHelper security)
        {
            this.context = context;
            this.security = security;
        }

        public bool IsUsernameFound(UserModel user)
        {
            return context.Users.Any(u => u.Username == user.Username);
        }

        public bool IsEmailFound(UserModel user)
        {
            return context.Users.Any(u => u.Email == user.Email);
        }

        public bool IsLoginValid(UserModel model)
        {
            UserModel user = context.Users.FirstOrDefault(u => u.Username == model.Username && u.Email == model.Email);
            if (user == null) return false;
            model.Password = security.HashPassword(model.Password, user.Salt);
            return model.Password == user.Password;
        }

        public UserModel GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public UserModel InsertUser(UserModel user)
        {
            user.Salt = security.GenerateSalt();
            user.Password = security.HashPassword(user.Password, user.Salt);
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public UserRoleModel InsertUserRole(UserRoleModel userRole)
        {
            context.UserRoles.Add(userRole);
            context.SaveChanges();
            return userRole;
        }

        public RoleModel GetRoleById(int id)
        {
            return context.Roles.Find(id);
        }

        public RoleModel GetRoleByName(string name)
        {
            return context.Roles.FirstOrDefault(r => r.Name == name);
        }

        public RoleModel GetRoleByUserId(int id)
        {
            UserRoleModel userRole = context.UserRoles.FirstOrDefault(u => u.UserId == id);
            return context.Roles.Find(userRole.RoleId);
        }

        /*public bool IsUsernameFound(UserModel user)
        {
            string statement = "SELECT username FROM dbo.Users WHERE username = $1";
            return SuccessQuery(user, statement);
        }

        public bool IsEmailFound(UserModel user)
        {
            string statement = "SELECT email FROM dbo.Users WHERE email = $1";
            return SuccessQuery(user, statement);
        }

        public UserModel GetUserByFullCredentials(UserModel user)
        {
            string statement = "SELECT * FROM dbo.Users WHERE username = @username AND email = $1 AND password = $2";
            string salt = FetchSaltQuery(user);

            if (salt == null) return null;
            user.Password = security.HashPassword(user.Password, salt);
            UserModel fetchedUser = FetchUserQuery(user, statement);
            return fetchedUser;
        }

        public int InsertUser(UserModel user)
        {
            string statement = "INSERT INTO dbo.Users (username, email, password, salt) OUTPUT Inserted.Id VALUES ($1, $2, $3, $4)";
            string salt = security.GenerateSalt();
            user.Password = security.HashPassword(user.Password, salt);
            int id = InsertQuery(user, statement, salt);
            return id;
        }

        public void InsertUserRole(int id, string roleName)
        {
            string statement = "SELECT * FROM dbo.Roles WHERE name = $1";
            RoleModel role = FetchRoleByNameQuery(roleName, statement);
            InsertUserRoleQuery(id, role.Id);
        }

        public string GetRoleByUserId(int id)
        {
            string statement = "SELECT roleId FROM dbo.UserRoles WHERE userId = $1";
            int roleId = FetchIdQuery(id, statement);
            statement = "SELECT * from dbo.Roles WHERE id = $1";
            RoleModel role = FetchRoleByIdQuery(roleId, statement);
            return role.Name;
        }

        public int InsertQuery(UserModel user, string statement, string salt)
        {
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            int id = -1;

            command.Parameters.AddWithValue("$1", user.Username);
            command.Parameters.AddWithValue("$2", user.Email);
            command.Parameters.AddWithValue("$3", user.Password);
            command.Parameters.AddWithValue("$4", salt);

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
            string statement = "INSERT into dbo.UserRoles VALUES ($1, $2)";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("$1", userId);
            command.Parameters.AddWithValue("$2", roleId);

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

        public bool SuccessQuery(UserModel user, string statement)
        {
            bool success = false;
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);

            command.Parameters.AddWithValue("$1", user.Username);
            command.Parameters.AddWithValue("$2", user.Email);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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


        public UserModel FetchUserQuery(UserModel user, string statement)
        {
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            UserModel fetchedUser = null;

            command.Parameters.AddWithValue("$1", user.Username);
            command.Parameters.AddWithValue("$2", user.Email);
            command.Parameters.AddWithValue("$3", user.Password);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
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

        public int FetchIdQuery(int id, string statement)
        {
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);            
            command.Parameters.AddWithValue("$1", id);
            int newId = -1;

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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

        public RoleModel FetchRoleByIdQuery(int roleId, string statement)
        {
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            command.Parameters.AddWithValue("$1", roleId);
            RoleModel role = new();

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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

        public RoleModel FetchRoleByNameQuery(string roleName, string statement)
        {
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            command.Parameters.AddWithValue("$1", roleName);
            RoleModel role = new();

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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

        public string FetchSaltQuery(UserModel user)
        {
            string statement = "SELECT salt FROM dbo.Users WHERE username = $1";
            using NpgsqlConnection connection = new(connectionString);
            NpgsqlCommand command = new(statement, connection);
            string salt = null;

            command.Parameters.AddWithValue("$1", user.Username);

            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

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
        }*/
    }
}
