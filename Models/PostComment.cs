using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{
    public class PostComment
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int PostId { get; set; }
        public string senderUsername { get; set; }
    }
}
