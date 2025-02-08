using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (!_authService.ValidateUser(user))
                return Unauthorized("Geçersiz kullanıcı adı veya şifre!");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
