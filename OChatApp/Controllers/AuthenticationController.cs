using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OChatApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetToken(
            [FromBody] LoginModel loginModel,
            [FromServices] OChatAppContext dbContext,
            [FromServices] SignInManager<OChatAppUser> signInManager,
            [FromServices] IConfiguration configuration)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null)
                return Unauthorized("User not found.");

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!signInResult.Succeeded)
                return Unauthorized("Sign in failed");

            var jsonWebToken = CreateJSONWebToken(user, configuration);

            return Ok(new { Token = jsonWebToken, UserId = user.Id, UserName = user.UserName});
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] LoginModel loginModel,
            [FromServices] UserManager<OChatAppUser> userManager)
        {
            var result = await RegisterUser(loginModel, userManager);

            if (result.Succeeded)
                return Ok(new { result = "Success" });

            var errors = new StringBuilder();

            foreach (var error in result.Errors)
                errors.Append(error.Description);

            return BadRequest(new { Result = $"Register Fail:{errors}" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromServices] SignInManager<OChatAppUser> signInManager)
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        private async Task<IdentityResult> RegisterUser(LoginModel loginModel, UserManager<OChatAppUser> userManager)
        {
            var newUser = new OChatAppUser()
            {
                UserName = loginModel.Email,
                Email = loginModel.Email,
                EmailConfirmed = true
            };

            return await userManager.CreateAsync(newUser, loginModel.Password);
        }

        private string CreateJSONWebToken(OChatAppUser user, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("JwtTokenKey").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
