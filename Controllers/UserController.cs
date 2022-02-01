using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TechBlog.Models;
using TechBlog.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {        
        readonly IUserDataService repo;
        readonly SecurityService security;

        public UserController(IUserDataService repo, SecurityService security)
        {
            this.repo = repo;
            this.security = security;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<string> IsAuthenticated()
        {
            return "User is authenticated";
        }

        [HttpPost("register")]
        public ActionResult<int> ProcessRegistration([FromBody] UserModel user)
        {
            try
            {
                if (repo.IsUsernameFound(user))
                {
                    throw new Exception("This username is taken. Please choose a different name.");
                }

                if (repo.IsEmailFound(user))
                {             
                    throw new Exception("This email address is already associated with an account. Either use a different email or login if you are the account holder.");
                }                           
                
                int id = repo.InsertUser(user);
                repo.InsertUserRole(id, "User");
                string roleName = repo.GetRoleByUserId(id);
                List<Claim> claims = security.AddClaims(user, roleName);
                SecurityToken token = security.GenerateToken(claims);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new 
                { 
                    Id = id, 
                    Token = tokenString, 
                    Expires = token.ValidTo 
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return Unauthorized(exc.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult<int> ProcessLogin([FromBody] UserModel user)
        {
            try
            {
                UserModel fetchedUser = repo.GetUserByFullCredentials(user);

                if (fetchedUser == null)
                {                
                    throw new Exception("Invalid account details. Please try again.");
                }

                string roleName = repo.GetRoleByUserId(fetchedUser.Id);
                List<Claim> claims = security.AddClaims(user, roleName);
                SecurityToken token = security.GenerateToken(claims);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    Id = fetchedUser.Id,
                    Token = tokenString,
                    Expires = token.ValidTo
                });
            }
            catch (Exception exc)
            {
                return Unauthorized(exc.Message);
            }
        }
    }
}
