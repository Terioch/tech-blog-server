using System.ComponentModel.DataAnnotations;

namespace TechBlog.Models
{
    public class RoleModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
