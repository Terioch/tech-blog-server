using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TechBlog.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TechBlog.Services
{
    public class SecurityHelper
    {
        private readonly IConfiguration config;

        public SecurityHelper(IConfiguration config)
        {
            this.config = config;
        }

        public string HashPassword(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
            byte[] bytesToBeHashed = Encoding.UTF8.GetBytes(salt + password);
            byte[] hashedPasswordBytes = algorithm.ComputeHash(bytesToBeHashed);
            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
            return hashedPassword;
        }

        // Generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
        public string GenerateSalt(int size=16)
        {
            RNGCryptoServiceProvider rng = new();
            byte[] buffer = new byte[size];
            rng.GetBytes(buffer);
            string salt = Convert.ToBase64String(buffer);
            return salt;
        }        

        // Create a JWT token for authorizing a user
        public SecurityToken GenerateToken(List<Claim> claims)
        {         
            var key = Encoding.ASCII.GetBytes(config["JWT_SECRET"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);                  
            return token;
        }

        public List<Claim> AddClaims(UserModel user, string roleName)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new(ClaimTypes.Role, roleName));
            return claims;
        }

        public string FindRoleForUser(UserModel user)
        {
            bool isUsernameMatch = user.Username == config["SuperAdminCredentials:Username"];
            bool isEmailMatch = user.Email == config["SuperAdminCredentials:Email"];
            bool isPasswordMatch = user.Password == config["SuperAdminCredentials:Password"];

            if (isUsernameMatch && isEmailMatch && isPasswordMatch)
            {
                return "Admin";
            }
            return "User";
        }
    }
}
