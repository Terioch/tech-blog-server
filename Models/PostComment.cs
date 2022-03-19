using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{
    public class PostComment
    {        
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [StringLength(400)]
        public string Value { get; set; }     

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string SenderUsername { get; set; }

        [Required]
        public long Date { get; set; }
    }
}
