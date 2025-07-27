using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using week2_Task.Models.DTOS;
using week2_Task.Models.Entities;
using week2_Task.Repositories;


namespace week2_Task.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly TokenService _tokenService;

        public AuthController(IAuthRepository authRepo, TokenService tokenService)
        {
            _authRepo = authRepo;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDTO dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validRoles = new[] { "Admin", "User" };
            if (string.IsNullOrEmpty(dto.Role) || !validRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Role must be either 'Admin' or 'User'.");
            }



            var user = new User
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            var createdUser = await _authRepo.CreateUserAsync(user);
            return Ok(new { message = "User created", createdUser.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _authRepo.GetUserAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }

}


