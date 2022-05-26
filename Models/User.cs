using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechBlog.DTOs;

namespace TechBlog.Models
{
    public class User
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "Username must be between 3 and 40 characters.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 40 characters.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be a minimum of 8 characters.")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Password must be a minimum of 8 characters.")]        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Salt { get; set; }       
    }
}
