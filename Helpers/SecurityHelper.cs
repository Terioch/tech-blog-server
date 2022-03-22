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
using TechBlog.Contexts;

namespace TechBlog.Services
{
    public class SecurityHelper
    {
        private readonly IConfiguration config;
        private readonly ISecurityDataService security;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly TechBlogDbContext context;

        public SecurityHelper(IConfiguration config, ISecurityDataService security, 
            TokenValidationParameters tokenValidationParameters, TechBlogDbContext context)
        {
            this.config = config;
            this.security = security;
            this.tokenValidationParameters = tokenValidationParameters;
            this.context = context;
        }

        public User RegisterUser(User model)
        {
            model.Salt = GenerateSalt();
            model.Password = HashPassword(model.Password, model.Salt);
            return security.InsertUser(model);
        }

        public bool IsLoginValid(User model)
        {
            User user = context.Users.FirstOrDefault(u => u.Username == model.Username && u.Email == model.Email);
            if (user == null) return false;
            model.Password = HashPassword(model.Password, user.Salt);
            return model.Password == user.Password;
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

        // Generate a JWT token for authorizing a user
        public SecurityToken GenerateAccessToken(List<Claim> claims)
        {         
            var key = Encoding.ASCII.GetBytes(config["JWT_SECRET"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);                  
            return token;
        }

        public RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken()
            {
                UserId = userId,
                Token = GenerateRefreshToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5000)
            };

            context.RefreshTokens.Add(refreshToken);
            context.SaveChanges();
            return refreshToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public AuthenticatedResult ProcessTokenRefresh(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromToken(accessToken);   
            var storedRefreshToken = context.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (principal == null || storedRefreshToken == null
                || DateTime.Now > storedRefreshToken.ExpiresAt)
            {
                throw new SecurityTokenException("Invalid Token");
            }
            
            // context.RefreshTokens.Update(storedRefreshToken);

            var user = security.GetUserById(int.Parse(principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var newAccessToken = GenerateAccessToken(principal.Claims.ToList()); 

            return new AuthenticatedResult()
            {
                Id = user.Id,
                Role = security.GetRoleByUserId(user.Id).Name,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = GenerateRefreshToken(user.Id).Token,
                Expires = newAccessToken.ValidTo
            };
        }

        private static bool IsJwtWithValidSecurityAlgorithm(JwtSecurityToken validatedToken)
        {
            /* return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                 jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, 
                     StringComparison.InvariantCultureIgnoreCase);*/
            return validatedToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenValidationParameters.ValidateLifetime = false; // An expired token should not be invalidated

            try
            {                
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken as JwtSecurityToken)) 
                {
                    return null;
                }
                return principal;
            } 
            catch
            {
                return null;
            }
        }

        public List<Claim> AddTokenClaims(User user, string roleName)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));           
            claims.Add(new(ClaimTypes.Role, roleName));
            return claims;
        }

        public string FindRoleForUser(User user)
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
