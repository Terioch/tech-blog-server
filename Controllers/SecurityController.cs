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

namespace TechBlog.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class SecurityController : ControllerBase
    {
        private readonly TechBlogDbContext context;
        readonly ISecurityDataService repo;
        readonly SecurityHelper security;

        public SecurityController(TechBlogDbContext context, ISecurityDataService repo, SecurityHelper security)
        {
            this.context = context;
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
        public ActionResult ProcessRegistration([FromBody] User model)
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
            User user = repo.InsertUser(model);                               
            UserRole userRole = new() { UserId = user.Id, RoleId = role.Id };
            repo.InsertUserRole(userRole);              
            List<Claim> claims = security.AddTokenClaims(user, role.Name);
            SecurityToken token = security.GenerateAccessToken(claims);            
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            RefreshToken refreshToken = security.GenerateRefreshToken(user.Id);

            return Ok(new AuthenticatedResult
            {
                Id = user.Id,
                Role = role.Name,
                AccessToken = tokenString,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            });
        }

        [HttpPost("login")]
        public ActionResult ProcessLogin([FromBody] User model)
        {            
            if (!repo.IsLoginValid(model))
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

            return Ok(new AuthenticatedResult
            {
                Id = user.Id,
                Role = role.Name,
                AccessToken = accessTokenString,
                RefreshToken = refreshToken.Token,
                ExpiresAt = accessToken.ValidTo
            });                                      
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> ProcessRefresh([FromBody] RefreshToken refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken.Token))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                AuthenticatedResult result = security.ProcessTokenRefresh(accessToken, refreshToken.Token);
                return Ok(result);
            }
            catch (Exception exc)
            {
                return Unauthorized(exc.Message);                
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout(int userId)
        {            
            var refreshToken = context.RefreshTokens.SingleOrDefault(t => t.UserId == userId);
            context.RefreshTokens.Remove(refreshToken);
            return Ok();
        }
    }
}
