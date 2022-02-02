using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{
    public class UserRoleModel
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public int RoleId { get; set; }
    }
}
