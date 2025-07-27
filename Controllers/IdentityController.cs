using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using week2_Task.Models;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;

namespace week2_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<APPUser> _userManager;
        private readonly TokenService _tokenService;

        public IdentityController(UserManager<APPUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser(NewUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var appUser = new APPUser
                {
                    UserName = user.Username,
                    Email = user.Email
                };

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validRoles = new[] { "Admin", "User" };
                if (string.IsNullOrEmpty(user.Role) || !validRoles.Contains(user.Role, StringComparer.OrdinalIgnoreCase))
                {
                    return BadRequest("Role must be either 'Admin' or 'User'.");
                }

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(appUser, user.Role);


                    return Ok("Registration successful");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
                return Unauthorized("User not found");

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!passwordCorrect)
                return Unauthorized("Incorrect password");

            
            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(new User
            {
                Id = user.Id, 
                Email = user.Email,
                Role = role
            });

            return Ok(new { token });
        }
    }
}
