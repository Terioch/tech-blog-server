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
using TechBlog.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class SecurityController : ControllerBase
    {
        private readonly TechBlogDbContext context;
        private readonly ISecurityDataService repo;
        private readonly SecurityHelper security;

        public SecurityController(TechBlogDbContext context, ISecurityDataService repo, SecurityHelper security)
        {
            this.context = context;
            this.repo = repo;
            this.security = security;
        }        

        [HttpPost("[action]")]
        public ActionResult Register([FromBody] User model)
        {                                  
            if (repo.IsUsernameFound(model))
            {                
                return Unauthorized(new UserException(
                    new UserError
                    {
                        Username = new string[] { "This username is taken. Please choose a different name." }
                    }, 401)
                );
            }

            if (repo.IsEmailFound(model))
            {                    
                return Unauthorized(new UserException(
                    new UserError
                    {
                        Email = new string[] { "This email address is already associated with an account. Either use a different email or login if you are the account holder." }
                    }, 401)
                );
            }

            Role role = repo.GetRoleByName("User");
            User user = security.RegisterUser(model);
            UserRole userRole = new() { UserId = user.Id, RoleId = role.Id };

            repo.InsertUserRole(userRole);              

            List<Claim> claims = security.AddTokenClaims(user, role.Name);
            SecurityToken accessToken = security.GenerateAccessToken(claims);            
            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            RefreshToken refreshToken = security.GenerateRefreshToken(user.Id);

            HttpContext.Response.Cookies.Append("access_token", accessTokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new AuthenticatedResult
            {
                Id = user.Id,
                Role = role.Name,
                AccessToken = accessTokenString,
                RefreshToken = refreshToken.Token,
                Expires = accessToken.ValidTo
            });
        }

        [HttpPost("[action]")]
        public ActionResult Login([FromBody] User model)
        {                        
            if (!security.IsLoginValid(model))
            {
                return Unauthorized(new UserException(
                    new UserError
                    {
                        General = new string[] { "Invalid account details. Please try again." }
                    }, 401)
                );
            }

            User user = repo.GetUserByEmail(model.Email);
            Role role = repo.GetRoleByUserId(user.Id);
            List<Claim> claims = security.AddTokenClaims(user, role.Name);
            SecurityToken accessToken = security.GenerateAccessToken(claims);            
            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            RefreshToken refreshToken = security.GenerateRefreshToken(user.Id);

            HttpContext.Response.Cookies.Append("access_token", accessTokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new AuthenticatedResult
            {
                Id = user.Id,
                Role = role.Name,
                AccessToken = accessTokenString,
                RefreshToken = refreshToken.Token,
                Expires = accessToken.ValidTo
            });                                      
        }

        [HttpPost("[action]")]
        public IActionResult Refresh([FromBody] RefreshToken refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken.Token))
                {
                    return Unauthorized();
                }

                // var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");                
                var accessToken = HttpContext.Request.Cookies["access_token"];
                // if (!string.IsNullOrEmpty(accessToken)) HttpContext.Request.Headers.Add("Authorization", "Bearer " + accessToken);
                AuthenticatedResult result = security.ProcessTokenRefresh(accessToken, refreshToken.Token);

                HttpContext.Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions 
                { 
                    HttpOnly = true, 
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                return Ok(result);
            }
            catch (Exception exc)
            {
                return Unauthorized(new UserException(
                    new UserError
                    {
                        General = new string[] { exc.Message }
                    }, 401)
                );
            }
        }

        [HttpPost("[action]/{id}")]
        public ActionResult Logout(int id)
        {            
            var refreshTokens = context.RefreshTokens.Where(t => t.UserId == id);
            context.RefreshTokens.RemoveRange(refreshTokens);
            context.SaveChanges();
            return Ok();
        }
    }
}
