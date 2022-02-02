using TechBlog.Models;

namespace TechBlog.Services
{
    public interface ISecurityDataService
    {
        public bool IsUsernameFound(UserModel user);        
        public bool IsEmailFound(UserModel user);        
        public bool IsLoginValid(UserModel model);
        public UserModel GetUserByEmail(string email);
        public UserModel InsertUser(UserModel user);       
        public UserRoleModel InsertUserRole(UserRoleModel userRole);
        public RoleModel GetRoleById(int id);
        public RoleModel GetRoleByName(string name);
        public RoleModel GetRoleByUserId(int id);       
    }
}
