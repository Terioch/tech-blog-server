﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{
    public class PostCommentModel
    {        
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Value { get; set; }      
        public string SenderUsername { get; set; }
        public long Date { get; set; }
    }
}
