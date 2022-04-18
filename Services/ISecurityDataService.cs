using TechBlog.Models;

namespace TechBlog.Services
{
    public interface ISecurityDataService
    {
        public bool IsUsernameFound(User user);        
        public bool IsEmailFound(User user);       
        public User GetUserById(int id);
        public User GetUserByEmail(string email);
        public User GetUserByUsername(string username);
        public User InsertUser(User user);       
        public UserRole InsertUserRole(UserRole userRole);
        public Role GetRoleById(int id);
        public Role GetRoleByName(string name);
        public Role GetRoleByUserId(int id);       
    }
}
