using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        // [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
