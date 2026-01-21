using IronSec.Models;
using IronSec.Services;
using Microsoft.AspNetCore.Mvc;

namespace IronSec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService = new AuthService();

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _authService.Register(user);

            return Ok(new UserResponse
            {
                Id = result.Id,
                Email = result.Email
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request.Email, request.Password);

            if (result == null)
                return Unauthorized("Email ou senha inválidos");

            return Ok(new UserResponse
            {
                Id = result.Id,
                Email = result.Email
            });
        }
    }
}
