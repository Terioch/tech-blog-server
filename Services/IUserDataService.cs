using TechBlog.Models;

namespace TechBlog.Services
{
    public interface IUserDataService
    {
        public bool IsUsernameFound(UserModel user);
        public bool IsEmailFound(UserModel user);
        public UserModel GetUserByFullCredentials(UserModel user);
        public int InsertUser(UserModel user);
        public void InsertUserRole(int id, string roleName);
        public string GetRoleByUserId(int id);
        public int InsertQuery(UserModel user, string statement, string salt);
        public void InsertUserRoleQuery(int userId, int roleId);
        public bool SuccessQuery(UserModel user, string statement);
        public UserModel FetchUserQuery(UserModel user, string statement);
        public int FetchIdQuery(int id, string statement);
        public RoleModel FetchRoleByIdQuery(int roleId, string statement);
        public RoleModel FetchRoleByNameQuery(string roleName, string statement);
        public string FetchSaltQuery(UserModel user);
    }
}
