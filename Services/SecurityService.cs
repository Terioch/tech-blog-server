using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
    }
}
