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

namespace TechBlog.Services
{
    public class SecurityService
    {
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

        public List<Claim> AddClaims(UserModel user, string roleName)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new(ClaimTypes.Role, roleName));
            return claims;
        }

        // Create a JWT token for authorizing a user
        public SecurityToken GenerateToken(List<Claim> claims)
        {         
            var key = Encoding.ASCII.GetBytes("MY_BIG_SECRET_KEY_LKHSFTYQFTSBDHF@($)(#)34324");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);                  
            return token;
        }
    }
}
