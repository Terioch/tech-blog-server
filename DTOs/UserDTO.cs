using System.ComponentModel.DataAnnotations;
using TechBlog.Models;

namespace TechBlog.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
       
        public string Username { get; set; }
        
        public string Email { get; set; }
    }
}
