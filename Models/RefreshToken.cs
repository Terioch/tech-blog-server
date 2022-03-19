﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TechBlog.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }        

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }        

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
