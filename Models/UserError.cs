using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Models
{   
    public class UserError
    {
        public string[] General { get; set; } = Array.Empty<string>();

        public string[] Username { get; set; } = Array.Empty<string>();

        public string[] Email { get; set; } = Array.Empty<string>();

        public string[] Password { get; set; } = Array.Empty<string>();
    }

    public class UserException : Exception
    {
        public UserException(UserError userErrorModel, int statusCode)
        {
            Errors = userErrorModel;
            Status = statusCode;
        }

        public int Status { get; private set; }

        public UserError Errors { get; private set; }
    }
}
