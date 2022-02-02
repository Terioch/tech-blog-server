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
            // return "Host=ec2-34-233-157-9.compute-1.amazonaws.com;Port=5432;Database=d7dh8fvadbcgvl;Username=lujeyzvxqmtgbu;Password=e873972673ea76fba3a3f81ba7d6075dec51f43d80d7f0e406b9dd025f426be6;Pooling=true;sslmode=require;Trust Server Certificate=true;";
        }
    }
}
