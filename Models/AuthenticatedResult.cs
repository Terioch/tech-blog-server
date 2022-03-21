using System;

namespace TechBlog.Models
{
    public class AuthenticatedResult
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Expires { get; set; }
    }
}
