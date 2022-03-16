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
    public class SecurityController : ControllerBase
    {        
        readonly ISecurityDataService repo;
        readonly SecurityHelper security;

        public SecurityController(ISecurityDataService repo, SecurityHelper security)
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
        public ActionResult ProcessRegistration([FromBody] UserModel model)
        {            
            try
            {
                if (repo.IsUsernameFound(model))
                {
                    throw new Exception("This username is taken. Please choose a different name.");
                }

                if (repo.IsEmailFound(model))
                {             
                    throw new Exception("This email address is already associated with an account. Either use a different email or login if you are the account holder.");
                }

                RoleModel role = repo.GetRoleByName("User");
                UserModel user = repo.InsertUser(model);                               
                UserRoleModel userRole = new() { UserId = user.Id, RoleId = role.Id };
                repo.InsertUserRole(userRole);              
                List<Claim> claims = security.AddClaims(user, role.Name);
                SecurityToken token = security.GenerateToken(claims);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new 
                { 
                    Id = user.Id, 
                    Role = role.Name,
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
        public ActionResult ProcessLogin([FromBody] UserModel model)
        {
            try
            {
                if (!repo.IsLoginValid(model))
                {                
                    throw new Exception("Invalid account details. Please try again.");
                }

                UserModel user = repo.GetUserByEmail(model.Email);
                RoleModel role = repo.GetRoleByUserId(user.Id);
                List<Claim> claims = security.AddClaims(user, role.Name);
                SecurityToken token = security.GenerateToken(claims);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    Id = user.Id,
                    Role = role.Name,
                    Token = tokenString,
                    Expires = token.ValidTo
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return Unauthorized(exc.Message);
            }
        }
    }
}
