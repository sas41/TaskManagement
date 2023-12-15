using Microsoft.AspNetCore.Mvc;
using TaskManagementBE.Services;
using TaskManagementBE.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace TaskManagementBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Create(User newUser)
        {
            var result = await _userManager.CreateAsync(newUser);
            var user = await _userManager.FindByIdAsync(newUser.Id.ToString());
            var vm = new UserViewModel(user, await _userManager.GetRolesAsync(user));
            if (result.Succeeded)
            {
                return Ok(vm);
            }
            return Unauthorized();
        }

        [HttpPut]
        public async Task<ActionResult<UserViewModel>> Update(User newUser)
        {
            var user = await _userManager.FindByIdAsync(newUser.Id.ToString());
            var vm = new UserViewModel(user, await _userManager.GetRolesAsync(user));
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(vm);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserViewModel>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var vm = new UserViewModel(user, await _userManager.GetRolesAsync(user));
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(vm);
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return Ok(new UserViewModel(user, await _userManager.GetRolesAsync(user)));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                users.Add(new UserViewModel(user, roles));

            }
            return Ok(users);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] AuthRequest credentials)
        {
            var username = credentials.Username;
            var password = credentials.Password;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Unauthorized(new AuthResponse { Success=false, Error= "Invalid username" });
            if (!await _userManager.CheckPasswordAsync(user, password))
                return Unauthorized(new AuthResponse { Success = false, Error = "Invalid password" });

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim("id", user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            string token = GenerateToken(authClaims) ?? "ERROR";
            return Ok(new AuthResponse { Success = true, Token = token, Roles = string.Join(';', userRoles), Username = user.UserName, Id = user.Id.ToString() });
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var bearer = _configuration.GetSection("Authentication:Schemes:Bearer");
            if (bearer is not null)
            {
                var uniqueKey = Encoding.UTF8.GetBytes(bearer["SigningKey"]);
                var issuers = bearer.GetSection("ValidIssuers")
                  .Get<string[]>();
                var audiences = bearer.GetSection("ValidAudiences")
                  .Get<string[]>();
                var lifetime = int.Parse(bearer["Lifetime"] ?? "2400");
                var key = new SymmetricSecurityKey(uniqueKey);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = issuers.First(),
                    Audience = audiences.First(),
                    Expires = DateTime.UtcNow.AddHours(lifetime),
                    SigningCredentials = creds,
                    Subject = new ClaimsIdentity(claims)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            return null;
        }
    }
}
