using Microsoft.Extensions.Configuration;
using System;

namespace TechBlog.Utility
{
    public class DataAccessHelper
    {
        private readonly IConfiguration config;

        public DataAccessHelper(IConfiguration config)
        {
            this.config = config;
        }

        public string GetConnectionString()
        {
            string databaseUrl = config["DATABASE_URL"];
            Uri uri = new Uri(databaseUrl);
            return $"host={uri.Host};username={uri.UserInfo.Split(':')[0]};password={uri.UserInfo.Split(':')[1]};database={uri.LocalPath.Substring(1)};sslmode=Prefer;Trust Server Certificate=true";                        
        }
    }
}
